using AutoMapper;
using CosmosManagementApi.Dtos;
using CosmosManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosManagementApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly ILogger<ProductController> _logger;
    private readonly CosmosManagementDbContext _context;
    private readonly IMapper _mapper;

    public ProductController(ILogger<ProductController> logger, CosmosManagementDbContext context, IMapper mapper)
    {
      _logger = logger;
      _context = context;
      _mapper = mapper;
    }

    // GET: api/<ProductController>
    //获取产品
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpGet]
    public IActionResult Get()
    {
      var result = _context.Products.Where(p => p.IfSelled == 0).ToList();
      var map = _mapper.Map<IEnumerable<ProductGetDto>>(result);
      var r_json = new
      {
        total = map.Count(),
        totalNotFiltered = map.Count(),
        rows = map
      };
      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }
    
    // GET: api/<ProductController>
    //获取库存
    [HttpGet("CheckStorage")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult GetStorage()
    {
      var result = _context.ProductClasses.Where(p => p.Storage <= 5).ToList();
      var map = _mapper.Map<IEnumerable<ProductClassDto>>(result);
      var r_json = new
      {
        total = map.Count(),
        totalNotFiltered = map.Count(),
        rows = map
      };
      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }

    // GET api/<ProductController>/5
    //获取单个产品
    [HttpGet("{id}")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult Get(int id)
    {
      if(id == 0){
        return BadRequest("Bad request");
      }
      var result = _context.Products.Find(id);
      if(result == null){
        return NotFound("未找到产品");
      }
      var map = _mapper.Map<ProductGetDto>(result);
      var r_json = new
      {
        total = 1,
        totalNotFiltered = 1,
        rows = map
      };
      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }

    //返还产品种类
    // GET: api/GetProdutClass<ProductController>
    [HttpGet("GetProdutClass")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IEnumerable<ProductClassDto> GetProductClass()
    {
      var result = _context.ProductClasses.ToList();
      var map = _mapper.Map<IEnumerable<ProductClassDto>>(result);
      return map;
    }

    //返还产品种类下所有产品
    // GET: api/GetProdutsInClass<ProductController>
    [HttpGet("GetProdutsInClass")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult GetProdutsInClass(int classId)
    {
      var result = _context.Products.Join(_context.ProductCategories, a => a.Id, b => b.ProductId, (a, b) => //join 两表 获取数据
       new ProductGetDto
       {
         ProductDate = a.ProductDate,
         ProductEndDate = a.ProductEndDate,
         IfSelled = a.IfSelled,
         CategoryId = b.ClassId,
       }
      ).Where(o => o.CategoryId == classId && o.IfSelled == 0).ToList(); //判断条件 选择相应class

      var r_json = new
      {
        total = result.Count(),
        totalNotFiltered = result.Count(),
        rows = result
      };
      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }

    //增加相应种类下产品数 并计入入库账单
    // POST api/<ProductController>
    [HttpPost("AddProduct")]
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] ProductAddDto value)
    {
      if (value == null)
      {
        return BadRequest("bad request");
      }
      var productClass = _context.ProductClasses.Find(value.ClassId);
      if (productClass == null){
        return NotFound("请求的产品种类不存在");
      }; //确认产品种类存在

      for (int i = 0; i < value.Number; i++)
      {
        InsertProduct(value); //调用函数 增加相应种类产品 函数内保存变更
      }

      productClass = _context.ProductClasses.Find(value.ClassId);
      productClass.Storage += value.Number;
      _context.SaveChanges();
      return Ok(productClass); //返还库存数量
    }

    //增加新的产品种类 同时计入入库账单
    [HttpPost("AddNewProduct")]
    [Authorize(Roles = "Admin")]
    public IActionResult PostNew([FromBody] ProductClassAddDto value)
    {
      if (value == null) {
        return BadRequest("bad request");
      }
      if(value.Storage == null)
      {
        value.Storage = 0; //如果用户传来的产品数为null 则改为0
      }
      var map = _mapper.Map<ProductClass>(value); //先增加产品种类，获取新产品种类所属的id 用于之后与新增产品绑定
      _context.ProductClasses.Add(map);
      _context.SaveChanges();

      ProductAddDto product = new ProductAddDto
      {
        ProductDate = value.ProductDate,
        ProductEndDate = value.ProductEndDate,
        ClassId = map.Id,
        Number = value.Storage,
        Price = value.BuyingPrice,
      };
      for(int i = 0; i < map.Storage; i++)
      {
        InsertProduct(product); //调用函数 增加相应种类产品 函数内保存变更
      }

      ProductClassDto result = new ProductClassDto
      {
        Id = map.Id,
        ProductName = map.ProductName,
        ProductVolume = map.ProductVolume,
        Introduction = map.Introduction,
        Img = map.Img,
        BuyingPrice = map.BuyingPrice,
        SellingPrice = map.SellingPrice,
        Category = map.Category,
        Storage = map.Storage,
        ProductId = map.ProductId,
      };

      return Ok(result); //返还新增产品数据及库存数量
    }

    // PUT api/<ProductController>/5
    //修改产品信息
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Put(int id, [FromBody] ProductClassUpdateDto value)
    {
      var update = _context.ProductClasses.Find(id);
      //if (update == null || update.IsDeleted == 1)
      //{
      //  return NotFound();
      //}
      if (update == null)
      {
        return NotFound();
      }
      else
      {
        _mapper.Map(value, update); //AutoMapper 更新Customer

        _context.SaveChanges(); //保存变更
      }

      return Ok(update);
    }

    // DELETE api/<ProductController>/5
    [HttpDelete("{id}")]
    [Authorize]
    public void Delete(int id)
    {
    }

    private void InsertProduct(ProductAddDto value)
    {
      var product = _mapper.Map<Product>(value);
      product.IfSelled = 0;
      _context.Products.Add(product);
      _context.SaveChanges(); //保存参数中的产品，保存更改 获取产品id

      ProductCategory pc = new ProductCategory
      {
        ClassId = value.ClassId,
        ProductId = product.Id,
        InStorageTime = DateTime.Today,
      }; //添加产品划分种类关系

      _context.ProductCategories.Add(pc);
      _context.SaveChanges(); //保存更新 至此函数完成 需注意是否需要异步完成


      string lastbillid = _context.Bills.OrderBy(e => e.Id).Last().BillId ?? "HG000000";
      int lastbillidnum = Convert.ToInt32(Regex.Replace(lastbillid, "[a-z]", "", RegexOptions.IgnoreCase));
      string billNumberInit = "HG" + string.Format("{0:D6}", lastbillidnum + 1);//公用的单号

      //开始记录账单
      Bill bill = new Bill {
        BillId = billNumberInit,
        OriPrice = value.Price,
        Discount = "1",
        FinalPrice = value.Price,
        Time = DateTime.Now,
        Comment = "商品进库存",
      };
      _context.Bills.Add(bill);
      _context.SaveChanges();

      ProductStorageBill psb = new ProductStorageBill
      {
        ProductId = product.Id,
        BillId = bill.Id,
      };
      _context.ProductStorageBills.Add(psb);
      _context.SaveChanges();
    }
  }
}

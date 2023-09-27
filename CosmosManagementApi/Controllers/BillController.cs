using AutoMapper;
using CosmosManagementApi.Dtos;
using CosmosManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosManagementApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BillController : ControllerBase
  {
    private readonly ILogger<BillController> _logger;
    private readonly CosmosManagementDbContext _context;
    private readonly IMapper _mapper;


    public BillController(ILogger<BillController> logger, CosmosManagementDbContext context, IMapper mapper)
    {
      _logger = logger;
      _context = context;
      _mapper = mapper;

    }

    // GET: api/<BillController>
    //获取所有Bill表单
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Get()
    {
      var result = _context.Bills.ToList();
      var map = _mapper.Map<IEnumerable<BillDto>>(result);
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
    //获取一个月内的bill表单
    [Authorize(Roles = "Admin")]
    [HttpGet("AllBills/{month}")]
    public IActionResult GetAllBills(int month)
    {
    if(month == 13){
     var CustomerProductBillTable = _context.Bills.Join(_context.CustomerProductBills, bill => bill.Id, cprodutbill => cprodutbill.BillId, (bill, cproductbill) => new {bill, cproductbill })
      .Join(_context.Customers, bcprb => bcprb.cproductbill.CustomerId, customer => customer.Id, (bcprd, customer) => new {bcprd, customer})
      .Select(m => new
      {
        Date = m.bcprd.bill.Time,
        BillNo = m.bcprd.bill.BillId,
        CustomerId = m.customer.CustomerId,
        CustomerName = m.customer.NameCn,
        PaymentMethod = m.bcprd.cproductbill.PaymentMethod,
        AmountSum = m.bcprd.bill.FinalPrice,
        Comment = m.bcprd.bill.Comment,
        BillId = m.bcprd.bill.Id,
        ProductId = m.bcprd.cproductbill.ProductId,
        StaffId = m.bcprd.cproductbill.StaffId,
        Time = m.bcprd.bill.Time,//分月份来
      }).ToList();

      var ProductTable = _context.Products
      .Join(_context.ProductCategories, product => product.Id, pcategory => pcategory.ProductId, (product, pcategory) => new{product, pcategory})
      .Join(_context.ProductClasses, pp => pp.pcategory.ClassId, pclass => pclass.Id, (pp, pclass) => new{pp, pclass })
      .Select( m => new{
        ProductName = m.pclass.ProductName,
        ProductId = m.pp.product.Id,
      }).ToList(); //产品的名字

      var StaffTable = _context.Staffs
      .Join(_context.Departments, staff => staff.DepartmentId, department => department.Id, (staff, department) => new{staff, department})
      .Select(m => new 
      {
        StaffId = m.staff.Id,
        StaffName = m.staff.StaffName,
        Department = m.department.DepartmentName,
      }).ToList();//员工的表

            var CustomerProjectBillTable = _context.Bills.Join(_context.CustomerProjectBills, bill => bill.Id, cprodutbill => cprodutbill.BillId, (bill, cproductbill) => new { bill, cproductbill })
      .Join(_context.Customers, bcprb => bcprb.cproductbill.CustomerId, customer => customer.Id, (bcprd, customer) => new { bcprd, customer })
      .Join(_context.Projects, c => c.bcprd.cproductbill.ProjectId, product => product.Id, (c, product) => new { c, product })
      .Select(m => new
      {
          Date = m.c.bcprd.bill.Time,
          BillNo = m.c.bcprd.bill.BillId,
          CustomerId = m.c.customer.CustomerId,
          CustomerName = m.c.customer.NameCn,
          PaymentMethod = m.c.bcprd.cproductbill.PaymentMethod,
          AmountSum = m.c.bcprd.bill.FinalPrice,
          Comment = m.c.bcprd.bill.Comment,
          ProjectName = m.product.ProjectName,
          ProductId = m.product.Id,
          BillId = m.c.bcprd.bill.Id,
          StaffId = m.c.bcprd.cproductbill.Staffid,
          Time = m.c.bcprd.bill.Time,
      }).ToList();

      var productTableFinal = CustomerProductBillTable.Join(ProductTable, cpbt => cpbt.ProductId, pt => pt.ProductId, (cpbt, pt)=> new{cpbt, pt})
      .Join(StaffTable, cpbtpt => cpbtpt.cpbt.StaffId, staff => staff.StaffId, (cpbtpt, staff) => new{cpbtpt, staff})
      .Select(m => new AllBillsDto
      {
          Date = m.cpbtpt.cpbt.Date,
          BillNo = m.cpbtpt.cpbt.BillNo,
          CustomerNo = m.cpbtpt.cpbt.CustomerId,
          CustomerName = m.cpbtpt.cpbt.CustomerName,
          Name = m.cpbtpt.pt.ProductName,
          PaymentMethod = m.cpbtpt.cpbt.PaymentMethod,
          AmountSum = m.cpbtpt.cpbt.AmountSum,
          Department = m.staff.Department,
          Comment = m.cpbtpt.cpbt.Comment,
          StaffName = m.staff.StaffName,
          BillType = "产品"
      }).ToList();

      var projectTableFinal = CustomerProjectBillTable
      .Join(StaffTable, cpbtpt => cpbtpt.StaffId, staff => staff.StaffId, (cpbtpt, staff) => new{cpbtpt, staff})
      .Select(m => new AllBillsDto
      {
          Date = m.cpbtpt.Date,
          BillNo = m.cpbtpt.BillNo,
          CustomerNo = m.cpbtpt.CustomerId,
          CustomerName = m.cpbtpt.CustomerName,
          Name = m.cpbtpt.ProjectName,
          PaymentMethod = m.cpbtpt.PaymentMethod,
          AmountSum = m.cpbtpt.AmountSum,
          Department = m.staff.Department,
          Comment = m.cpbtpt.Comment,
          StaffName = m.staff.StaffName,
          BillType = "项目"
      }).ToList();

      productTableFinal.AddRange(projectTableFinal);
      var r_json = new
      {
        total = productTableFinal.Count(),
        totalNotFiltered = productTableFinal.Count(),
        rows = productTableFinal
      };//这个格式是为了方便前端显示
      return new JsonResult(r_json){

                StatusCode = 200,
      };
    }
    else{ //按月份选择
           var CustomerProductBillTable = _context.Bills.Join(_context.CustomerProductBills, bill => bill.Id, cprodutbill => cprodutbill.BillId, (bill, cproductbill) => new {bill, cproductbill })
      .Join(_context.Customers, bcprb => bcprb.cproductbill.CustomerId, customer => customer.Id, (bcprd, customer) => new {bcprd, customer})
      .Select(m => new
      {
        Date = m.bcprd.bill.Time,
        BillNo = m.bcprd.bill.BillId,
        CustomerId = m.customer.CustomerId,
        CustomerName = m.customer.NameCn,
        PaymentMethod = m.bcprd.cproductbill.PaymentMethod,
        AmountSum = m.bcprd.bill.FinalPrice,
        Comment = m.bcprd.bill.Comment,
        BillId = m.bcprd.bill.Id,
        ProductId = m.bcprd.cproductbill.ProductId,
        StaffId = m.bcprd.cproductbill.StaffId,
        Time = m.bcprd.bill.Time,//分月份来
      }).Where(j => j.Time.Value.Month == month).ToList();

      var ProductTable = _context.Products
      .Join(_context.ProductCategories, product => product.Id, pcategory => pcategory.ProductId, (product, pcategory) => new{product, pcategory})
      .Join(_context.ProductClasses, pp => pp.pcategory.ClassId, pclass => pclass.Id, (pp, pclass) => new{pp, pclass })
      .Select( m => new{
        ProductName = m.pclass.ProductName,
        ProductId = m.pp.product.Id,
      }).ToList(); //产品的名字

      var StaffTable = _context.Staffs
      .Join(_context.Departments, staff => staff.DepartmentId, department => department.Id, (staff, department) => new{staff, department})
      .Select(m => new 
      {
        StaffId = m.staff.Id,
        StaffName = m.staff.StaffName,
        Department = m.department.DepartmentName,
      }).ToList();//员工的表

            var CustomerProjectBillTable = _context.Bills.Join(_context.CustomerProjectBills, bill => bill.Id, cprodutbill => cprodutbill.BillId, (bill, cproductbill) => new { bill, cproductbill })
      .Join(_context.Customers, bcprb => bcprb.cproductbill.CustomerId, customer => customer.Id, (bcprd, customer) => new { bcprd, customer })
      .Join(_context.Projects, c => c.bcprd.cproductbill.ProjectId, product => product.Id, (c, product) => new { c, product })
      .Select(m => new
      {
          Date = m.c.bcprd.bill.Time,
          BillNo = m.c.bcprd.bill.BillId,
          CustomerId = m.c.customer.CustomerId,
          CustomerName = m.c.customer.NameCn,
          PaymentMethod = m.c.bcprd.cproductbill.PaymentMethod,
          AmountSum = m.c.bcprd.bill.FinalPrice,
          Comment = m.c.bcprd.bill.Comment,
          ProjectName = m.product.ProjectName,
          ProductId = m.product.Id,
          BillId = m.c.bcprd.bill.Id,
          StaffId = m.c.bcprd.cproductbill.Staffid,
          Time = m.c.bcprd.bill.Time,
      }).Where(j => j.Time.Value.Month == month).ToList();

      var productTableFinal = CustomerProductBillTable.Join(ProductTable, cpbt => cpbt.ProductId, pt => pt.ProductId, (cpbt, pt)=> new{cpbt, pt})
      .Join(StaffTable, cpbtpt => cpbtpt.cpbt.StaffId, staff => staff.StaffId, (cpbtpt, staff) => new{cpbtpt, staff})
      .Select(m => new AllBillsDto
      {
          Date = m.cpbtpt.cpbt.Date,
          BillNo = m.cpbtpt.cpbt.BillNo,
          CustomerNo = m.cpbtpt.cpbt.CustomerId,
          CustomerName = m.cpbtpt.cpbt.CustomerName,
          Name = m.cpbtpt.pt.ProductName,
          PaymentMethod = m.cpbtpt.cpbt.PaymentMethod,
          AmountSum = m.cpbtpt.cpbt.AmountSum,
          Department = m.staff.Department,
          Comment = m.cpbtpt.cpbt.Comment,
          StaffName = m.staff.StaffName,
          BillType = "产品"
      }).ToList();

      var projectTableFinal = CustomerProjectBillTable
      .Join(StaffTable, cpbtpt => cpbtpt.StaffId, staff => staff.StaffId, (cpbtpt, staff) => new{cpbtpt, staff})
      .Select(m => new AllBillsDto
      {
          Date = m.cpbtpt.Date,
          BillNo = m.cpbtpt.BillNo,
          CustomerNo = m.cpbtpt.CustomerId,
          CustomerName = m.cpbtpt.CustomerName,
          Name = m.cpbtpt.ProjectName,
          PaymentMethod = m.cpbtpt.PaymentMethod,
          AmountSum = m.cpbtpt.AmountSum,
          Department = m.staff.Department,
          Comment = m.cpbtpt.Comment,
          StaffName = m.staff.StaffName,
          BillType = "项目"
      }).ToList();

      productTableFinal.AddRange(projectTableFinal);
      var r_json = new
      {
        total = productTableFinal.Count(),
        totalNotFiltered = productTableFinal.Count(),
        rows = productTableFinal
      };//这个格式是为了方便前端显示
      return new JsonResult(r_json)
      {
         StatusCode = 200,
      };

    }
    }

    //获取所有用户购买过的产品的账单
    [Authorize(Roles = "Admin")]
    [HttpGet("CustomerProductBills")]
    public IActionResult GetCustomerProduct()
    {
      var result = _context.Bills.Join(_context.CustomerProductBills,
      bill => bill.Id, cpb => cpb.BillId, (bill, cpb) => new { bill, cpb })
      .Join(_context.Products, productbill => productbill.cpb.ProductId, product => product.Id, (productbill, product) => new { productbill, product })
      .Join(_context.ProductCategories, ppb => ppb.product.Id, category => category.ProductId, (ppbc, category) => new { ppbc, category })
      .Join(_context.ProductClasses, d => d.category.ClassId, cass => cass.Id, (ppbcc, cass) => new { ppbcc, cass }) //join 两表 获取数据
      .Join(_context.Staffs, f => f.ppbcc.ppbc.productbill.cpb.StaffId, staff => staff.Id, (f, staff) => 
      new CustomerProductBillGetDto
      {
        BillId = f.ppbcc.ppbc.productbill.bill.BillId,
        OriPrice = f.ppbcc.ppbc.productbill.bill.OriPrice,
        Discount = f.ppbcc.ppbc.productbill.bill.Discount,
        FinalPrice = f.ppbcc.ppbc.productbill.bill.FinalPrice,
        Time = f.ppbcc.ppbc.productbill.bill.Time,
        Comment = f.ppbcc.ppbc.productbill.bill.Comment,
        Number = f.ppbcc.ppbc.productbill.cpb.Number,
        PaymentMethod = f.ppbcc.ppbc.productbill.cpb.PaymentMethod,
        CustomerId = f.ppbcc.ppbc.productbill.cpb.CustomerId,
        StaffId = f.ppbcc.ppbc.productbill.cpb.StaffId,
        StaffName = staff.StaffName,
        ProductId = f.ppbcc.ppbc.product.Id,
        Product = _mapper.Map<ProductClassDto>(f.cass)
      }
     ).ToList(); //获取所有用户购买产品的账单
      var r_json = new
      {
        total = result.Count(),
        totalNotFiltered = result.Count(),
        rows = result
      };//这个格式是为了方便前端显示

      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }

    //获取单个用户购买过的产品的账单
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpGet("CustomerProductBills/{id}")]
    public IActionResult GetACustomerProducts(int id)
    {
      var result = _context.Bills.Join(_context.CustomerProductBills,
      bill => bill.Id, cpb => cpb.BillId, (bill, cpb) => new { bill, cpb })
      .Where(gg => gg.cpb.CustomerId == id)
      .Join(_context.Products, productbill => productbill.cpb.ProductId, product => product.Id, (productbill, product) => new { productbill, product })
      .Join(_context.ProductCategories, ppb => ppb.product.Id, category => category.ProductId, (ppbc, category) => new { ppbc, category })
      .Join(_context.ProductClasses, d => d.category.ClassId, cass => cass.Id, (ppbcc, cass) => new { ppbcc, cass }) //join 两表 获取数据
      .Join(_context.Staffs, f => f.ppbcc.ppbc.productbill.cpb.StaffId, staff => staff.Id, (f, staff) =>
      new CustomerProductBillGetDto
      {
        BillId = f.ppbcc.ppbc.productbill.bill.BillId,
        OriPrice = f.ppbcc.ppbc.productbill.bill.OriPrice,
        Discount = f.ppbcc.ppbc.productbill.bill.Discount,
        FinalPrice = f.ppbcc.ppbc.productbill.bill.FinalPrice,
        Time = f.ppbcc.ppbc.productbill.bill.Time,
        Comment = f.ppbcc.ppbc.productbill.bill.Comment,
        Number = f.ppbcc.ppbc.productbill.cpb.Number,
        PaymentMethod = f.ppbcc.ppbc.productbill.cpb.PaymentMethod,
        CustomerId = f.ppbcc.ppbc.productbill.cpb.CustomerId,
        StaffId = f.ppbcc.ppbc.productbill.cpb.StaffId,
        StaffName = staff.StaffName,
        ProductId = f.ppbcc.ppbc.product.Id,
        Product = _mapper.Map<ProductClassDto>(f.cass)
      }
     ).ToList(); //获取所有用户购买产品的账单
      var r_json = new
      {
        total = result.Count(),
        totalNotFiltered = result.Count(),
        rows = result
      };//这个格式是为了方便前端显示

      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }

    //获取所有用户购买过的项目的账单
    [Authorize(Roles = "Admin")]
    [HttpGet("CustomerProjectBills")]
    public IActionResult GetCustomerProject()
    {
      var result = _context.Bills.Join(_context.CustomerProjectBills,
      bill => bill.Id, cpb => cpb.BillId, (bill, cpb) => new { bill, cpb })
      .Join(_context.Projects, c => c.cpb.ProjectId, project => project.Id, (c, project) => new{c, project})
      .Join(_context.Staffs, f => f.c.cpb.Staffid, staff => staff.Id, (f, staff) => //join 两表 获取数据
      new CustomerProjectBillGetDto
      {
        BillId = f.c.bill.BillId,
        OriPrice = f.c.bill.OriPrice,
        Discount = f.c.bill.Discount,
        FinalPrice = f.c.bill.FinalPrice,
        Time = f.c.bill.Time,
        Comment = f.c.bill.Comment,
        ProjectName = f.project.ProjectName,
        ProjectNumber = f.c.cpb.ProjectNumber,
        PaymentMethod = f.c.cpb.PaymentMethod,
        CustomerId = f.c.cpb.CustomerId,
        StaffId = f.c.cpb.Staffid,
        StaffName = staff.StaffName,

        ProjectId = f.project.Id,
        
        Project = f.project
      }
     ).ToList(); //获取所有用户购买产品的账单
      var r_json = new
      {
        total = result.Count(),
        totalNotFiltered = result.Count(),
        rows = result
      };//这个格式是为了方便前端显示

      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }

    //获取单个用户购买过的项目的账单
    [HttpGet("CustomerProjectBills/{id}")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult GetACustomerProjects(int id)
    {
      var result = _context.Bills.Join(_context.CustomerProjectBills,
      bill => bill.Id, cpb => cpb.BillId, (bill, cpb) => new { bill, cpb })
      .Where(gg => gg.cpb.CustomerId == id)
      .Join(_context.Projects, c => c.cpb.ProjectId, project => project.Id, (c, project) => new { c, project })
      .Join(_context.Staffs, f => f.c.cpb.Staffid, staff => staff.Id, (f, staff) => //join 两表 获取数据
      new CustomerProjectBillGetDto
      {
        BillId = f.c.bill.BillId,
        OriPrice = f.c.bill.OriPrice,
        Discount = f.c.bill.Discount,
        FinalPrice = f.c.bill.FinalPrice,
        Time = f.c.bill.Time,
        Comment = f.c.bill.Comment,
        ProjectName = f.project.ProjectName,
        ProjectNumber = f.c.cpb.ProjectNumber,
        PaymentMethod = f.c.cpb.PaymentMethod,
        CustomerId = f.c.cpb.CustomerId,
        StaffId = f.c.cpb.Staffid,
        StaffName = staff.StaffName,

        ProjectId = f.project.Id,
        CommentOfProjectLine = f.c.cpb.Comment,

        Project = f.project
      }
     ).ToList(); //获取所有用户购买产品的账单
      var r_json = new
      {
        total = result.Count(),
        totalNotFiltered = result.Count(),
        rows = result
      };//这个格式是为了方便前端显示

      JsonResult jsR = new JsonResult(r_json)
      {
        StatusCode = 200,
      };

      return jsR;
    }

    //用于传输用户同时购买产品和项目开单
    //POST api/<ProductController>
    //[Authorize(Roles = "O1Staff, Admin")]
    [HttpPost("CustomerBills")]
    public IActionResult PostCustomerBills([FromBody] CustomerBillsPostDto Bills)
    {
      if (Bills == null)
      {
        return BadRequest("传输的参数为空");
      }

      CustomerProductBillPostReturnDto cprdPD = new CustomerProductBillPostReturnDto();//用于第四部分返还账单 product
      CustomerProjectBillPostReturnDto cprdPJ = new CustomerProjectBillPostReturnDto();//用于第四部分返还project账单

      string lastbillid = _context.Bills.OrderBy(e => e.Id).LastOrDefault().BillId ?? "HG000000"; //获取最后一行的billId
      int lastbillidnum = Convert.ToInt32(Regex.Replace(lastbillid, "[a-z]", "", RegexOptions.IgnoreCase));
      string billNumberInit = "HG" + string.Format("{0:D6}", lastbillidnum + 1);//公用的单号 最后用到的单号

      
                //准备执行之后步骤 先生成单号
                Bill bill = new Bill();
                //billid 更新规则 从HG000001开始
                bill.OriPrice = Bills.OriPrice;
                bill.Discount = Bills.Discount;
                bill.FinalPrice = Bills.FinalPrice;
                bill.Comment = Bills.Comment;
                bill.Time = DateTime.Now; //注入bill数据
                bill.PaymentMethod = JsonSerializer.Serialize(Bills.PaymentMethods, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
                _context.Bills.Add(bill); //获得bill id
                bill.BillId = billNumberInit;


      //_context.SaveChanges();//储存 获得bill主键id

      if (Bills.ProductBills != null)//传输了Product 走product开单流程
      {
        ProductClass productClass = new ProductClass();//获取产品种类信息
        Customer customer = new Customer();//获取客户信息
        Staff staff = new Staff();//或许操作的staff的信息
        Product product = new Product();//用于随机获取产品



        var productTable = _context.ProductClasses.Join(_context.ProductCategories,
          productClass => productClass.Id, productCategory => productCategory.ClassId,
          (productClass, productCategory) => new { productClass, productCategory }) //productClass 和 productCategory 根据 product Class 主键join
          .Join(_context.Products, c => c.productCategory.ProductId, product => product.Id, (c, product) => new { c, product })
          .Where(j => j.product.IfSelled == 0) //选中未被售卖产品
          .Select(m => new
          {
            ProductName = m.c.productClass.ProductName,
            ProductVolume = m.c.productClass.ProductVolume,
            Introduction = m.c.productClass.Introduction,
            pId = m.product.Id,
            ProductId = m.c.productClass.ProductId,
            ProductClassId = m.c.productClass.Id,
          }).ToList(); //获取产品信息 获取product表中的信息

        //三方面检查
        foreach (var a in Bills.ProductBills)
        {
          productClass = _context.ProductClasses.Find(a.ProductId); //此处传输的是product class的id
          customer = _context.Customers.Find(a.CustomerId);
          staff = _context.Staffs.Find(a.StaffId);

          if (productClass == null || customer == null || staff == null)
          {
            return BadRequest("产品、客户或员工信息缺失");
          }

          if (productTable.Where(i => i.ProductClassId == a.ProductId).FirstOrDefault() == null)
          {
            return BadRequest("产品库无产品信息 或产品无库存，请检查");
          }

          if (productClass.Storage == 0 || productClass.Storage == null || productClass.Storage < a.Number)
          {
            return BadRequest("产品无库存或库存不足，请检查");
          }

        }//check if has null value if has return fail


        _context.SaveChanges();//储存 获得bill主键id

        //foreach (PaymentMethodDto pmto in Bills.PaymentMethods.Where(m => m.Method == "Card")){
        //    if (_context.Cards.Where(card => card.Id == pmto.CardId).First().Topped > pmto.Amount){
        //        var card = _context.Cards.Where(card => card.Id == pmto.CardId).First();
        //        CardBill CB = new CardBill()
        //        {
        //            CardId = card.Id,
        //            PaymentMethod = "卡支付部分账单",
        //            BillId = bill.Id,
        //        };
        // card.Topped -= Convert.ToDecimal(Bills.PaymentMethods.First().Amount);
        // _context.CardBills.Add(CB);
        // _context.SaveChanges();
        //    }else{
        //    return BadRequest("客户卡内余额不足或客户无此卡");
        //    }
        //}

        cprdPD = MakePayProduct(Bills.ProductBills, bill.Id ,billNumberInit);
      }
      if (Bills.ProjectBills != null)//传输了project 走project流程
      {
        Customer customer = new Customer();//获取客户信息
        CustomerProject CP = new CustomerProject();//获取项目次数
        foreach (var a in Bills.ProjectBills)
        {
          if (a.ProjectId == null || _context.Projects.Find(a.ProjectId) == null)
          {
            return BadRequest("请求的项目不存在");
          };
          customer = _context.Customers.Find(a.CustomerId);
          if (a.CustomerId == null || customer == null)
          {
            return BadRequest("请求的客户不存在");
          };
          if (a.StaffId == null || _context.Staffs.Find(a.StaffId) == null)
          {
            return BadRequest("请求的员工不存在");
          }; // 对每一项post数据进行检验
          if (a.ProjectNumber == 0)
          {
            return BadRequest("传输的使用/增加产品数为0 不规范");
          };
          CP = _context.CustomerProjects.Where(p => p.ProjectId == a.ProjectId && p.CustomerId == a.CustomerId).FirstOrDefault(); // 验证次数是否够
          if (a.ProjectNumber < 0 && CP != null && CP.Number < -a.ProjectNumber)
          {
            return BadRequest("用户拥有的项目次数不足，请先购买再请求");
          }
        }//验证完毕 准备走开单流程

                //Bill bill = new Bill();
                ////billid 更新规则 从HG000001开始
                //bill.OriPrice = Bills.OriPrice;
                //bill.Discount = Bills.Discount;
                //bill.FinalPrice = Bills.FinalPrice;
                //bill.Comment = Bills.Comment;
                //bill.Time = DateTime.Now; //注入bill数据
                //_context.Bills.Add(bill); //获得bill id
                //bill.BillId = billNumberInit;
                //_context.SaveChanges();

                //if (Bills.PaymentMethods.First().Method == "Card")
                //{
                //    if (_context.Cards.Where(card => card.Topped > 0).Any(card => card.CustomerId == Bills.CustomerId))
                //    {
                //        var card = _context.Cards.Where(card => card.CustomerId == Bills.CustomerId && card.Topped > 0).FirstOrDefault();
                //        CardBill CB = new CardBill()
                //        {
                //            CardId = card.Id,
                //            PaymentMethod = "卡支付部分账单",
                //            BillId = bill.Id,
                //        };
                //        card.Topped -= Convert.ToDecimal(Bills.PaymentMethods.First().Amount);
                //        _context.CardBills.Add(CB);
                //        //_context.SaveChanges();
                //    }
                //    else
                //    {
                //        return BadRequest("客户没有充值卡或卡内余额为零");
                //    }
                //}

        _context.SaveChanges();//储存 获得bill主键id

        cprdPJ = MakePayProject(Bills.ProjectBills, bill.Id, billNumberInit);

      }

            foreach (PaymentMethodDto pmto in Bills.PaymentMethods.Where(m => m.Method == "Card"))
            {
                if (_context.Cards.Where(card => card.Id == pmto.CardId).First().Topped > pmto.Amount)
                {
                    var card = _context.Cards.Where(card => card.Id == pmto.CardId).First();
                    CardBill CB = new CardBill()
                    {
                        CardId = card.Id,
                        PaymentMethod = "卡支付部分账单",
                        BillId = bill.Id,
                    };
                    card.Topped -= Convert.ToDecimal(Bills.PaymentMethods.First().Amount);
                    _context.CardBills.Add(CB);
                    
                }
                else
                {
                    return BadRequest("客户卡内余额不足或客户无此卡");
                }
            }
            _context.SaveChanges();

      CustomerBillsReturnDto cprd = new CustomerBillsReturnDto();
      cprd.ProductBillsReturn = cprdPD;
      cprd.ProjectBillsReturn = cprdPJ;


      //至此做完返还单据

      return Ok(cprd);
    }


    // PUT api/<ProductController>/5
    //修改表单备注
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpPut("EditComment/{id}")]
    public IActionResult Put(int id, [FromBody] string value)
    {
      var update = _context.Bills.Find(id);
      if (update == null)
      {
        return BadRequest("未找到单号");
      }
      else
      {
        update.Comment = value;
        _context.SaveChanges();
      }

      return Ok(update);
    }


    //修改project comment
    [HttpPut("EditProjectComment")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult EditProjectComment([FromBody] CommentEditDto value) 
    {
      if(value == null){
         return BadRequest("值为空");
      }
       var projectBill = _context.CustomerProjectBills.Where(m => m.ProjectId == value.PId)
        .Join(_context.Bills, cpb => cpb.BillId, bill => bill.Id, (cpb, bill) => new { cpb, bill})
        .Where(n => n.bill.BillId == value.BillId).First();
       
      if(projectBill == null){
        return BadRequest("未找到对应账单");
      }
       var pb_des = _context.CustomerProjectBills.Find(projectBill.cpb.Id);
       pb_des.Comment = value.Comment;
       _context.SaveChanges();
       return Ok("修改成功");
    }

    // DELETE api/<ProductController>/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var delete = _context.Projects.Find(id);
      if (delete == null)
      {
        return NotFound();
      }
      delete.IsDeleted = 1;
      _context.SaveChanges();
      return Ok("项目记录已删除");
    }

    //private IQueryable<int> FindProduct(int productId)
    //{
    //  int? classId = _context.ProductCategories.Where(a => a.ProductId == productId).First().ClassId; //可能会有空值报错
    //  IQueryable<int> query = _context.ProductClasses.Find(classId);
    //  return query;
    //}

    private Project findProject(int projectId)
    {
      Project p = _context.Projects.Find(projectId);
      return p;
    }

    private CustomerProductBillPostReturnDto MakePayProduct(List<CustomerProductBillPostDto> value, int idBill, string BillNumberInit)
    {
      CustomerProductBillPostReturnDto cprd = new CustomerProductBillPostReturnDto();//用于第四部分返还账单

      ProductClass productClass = new ProductClass();//获取产品种类信息
      Customer customer = new Customer();//获取客户信息
      Staff staff = new Staff();//或许操作的staff的信息
      Product product = new Product();//用于随机获取产品


      var productTable = _context.ProductClasses.Join(_context.ProductCategories,
        productClass => productClass.Id, productCategory => productCategory.ClassId,
        (productClass, productCategory) => new { productClass, productCategory }) //productClass 和 productCategory 根据 product Class 主键join
      .Join(_context.Products, c => c.productCategory.ProductId, product => product.Id, (c, product) => new { c, product })
      .Where(j => j.product.IfSelled == 0) //选中未被售卖产品
      .Select(m => new
      {
        ProductName = m.c.productClass.ProductName,
        ProductVolume = m.c.productClass.ProductVolume,
        Introduction = m.c.productClass.Introduction,
        pId = m.product.Id,
        ProductId = m.c.productClass.ProductId,
        ProductClassId = m.c.productClass.Id,
      }).ToList(); //获取产品信息 获取product表中的信息

      //创建List用于接受invoices
      List<InvoiceDto> cprdInvoices = new List<InvoiceDto>();


      //储存账单信息 出货 出库
      foreach (var a in value)
      {
        //if (_context.Cards.Where(card => card.Topped > 0).Any(card => card.CustomerId == a.CustomerId)){
        //  var card = _context.Cards.Where(card => card.CustomerId == a.CustomerId && card.Topped > 0).FirstOrDefault();
        //  CardBill CB = new CardBill()
        //  {
        //    CardId = card.Id,
        //    PaymentMethod = "卡支付部分账单",
        //    StaffId = a.StaffId,
        //  };
        //  _context.SaveChanges();// 保存CardBill BillId未存

          //Bill bill = new Bill();
          ////billid 更新规则 从HG000001开始
          //bill.OriPrice = a.OriPrice;
          //bill.Discount = a.Discount;
          ////bill.FinalPrice = Convert.ToString(Convert.ToInt32(a.FinalPrice) - card.Topped > 0 ? Convert.ToInt32(a.FinalPrice) - card.Topped : 0); //注意下相等情况
          //bill.Time = DateTime.Now;
          //bill.Comment = a.Comment; //进入loop循环，每次循环更新bill内容 并添加进相应表

          //_context.Bills.Add(bill);
          //_context.SaveChanges();           //保存 以此获得bill的id 传给CustomerProductBill

          //bill.BillId = BillNumberInit;

          //CB.BillId = bill.Id;

          //int? ori_Topped = card.Topped;

          //card.Topped = card.Topped - Convert.ToInt32(a.FinalPrice) > 0 ? card.Topped - Convert.ToInt32(a.FinalPrice) : 0;
          //_context.SaveChanges();//更新完卡的topped



          CustomerProductBill cpb = new CustomerProductBill();//用于第二部分记录账单
          cpb.CustomerId = a.CustomerId;
          cpb.ProductId = a.ProductId; //产品class的id
          cpb.BillId = idBill;
          cpb.Number = a.Number;
          cpb.UnitPrice = a.UnitPrice;
          cpb.NetPrice = a.NetPrice;
          cpb.Discount = a.Discount;
          //cpb.PaymentMethod = JsonSerializer.Serialize(a.PaymentMethods, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });

          cpb.StaffId = a.StaffId;

          _context.CustomerProductBills.Add(cpb);
          //_context.SaveChanges(); //保存 CustomerProductBill信息，完成任务，请考虑异步 同步

          productClass = _context.ProductClasses.Find(a.ProductId);
          customer = _context.Customers.Find(a.CustomerId);
          staff = _context.Staffs.Find(a.StaffId);
          product = _context.Products.Find(productTable.Where(i => i.ProductClassId == a.ProductId).FirstOrDefault().pId); //找相应产品种类id的产品 取第一个
          product.IfSelled = 1;
          productClass.Storage -= 1;
          //_context.SaveChanges();//完成出货 出库

          cprdInvoices.Add(new InvoiceDto
          {
            ItemNo = productClass.ProductId,
            Description = productClass.ProductName,
            UnitPrice = a.UnitPrice,
            NetPrice = a.NetPrice,
            Staff = staff.StaffName,
            StaffNumber = staff.StaffId,
            PaymentMethods = cpb.PaymentMethod,
          });
      }
      cprd.InvoiceNo = BillNumberInit;
      cprd.Date = DateTime.Now;
      cprd.CustomerNo = customer.CustomerId;
      cprd.CustomerName = customer.NameCn;
      cprd.Invoices = cprdInvoices;
      return cprd;
    }


    private CustomerProjectBillPostReturnDto MakePayProject(List<CustomerProjectBillPostDto> value, int idBill,string BillNumberInit)
    {

      CustomerProjectBillPostReturnDto cprd = new CustomerProjectBillPostReturnDto();
      Customer customer = new Customer();//获取客户信息
      CustomerProject CP = new CustomerProject();//获取项目次数
      List<InvoiceDto> cprdInvoices = new List<InvoiceDto>(); // 用于记录账单

      //开始插入数据 从Bills表开始 到CustomerProjectBills 到 CustomerProjects
      foreach (var a in value)
      {
        //if(_context.Cards.Where(card => card.Topped >0 ).Any(card => card.CustomerId == a.CustomerId))
        //{//有卡且卡有余额执行的步骤
          //var card = _context.Cards.Where(card => card.CustomerId == a.CustomerId && card.Topped >0).FirstOrDefault();
          //CardBill CB = new CardBill()
          //{
          //  CardId = card.Id,
          //  PaymentMethod = "卡支付部分账单",
          //  StaffId = a.StaffId,
          //};
          //_context.SaveChanges();// 保存CardBill BillId未存
          //Bill bill = new Bill();
          //bill.OriPrice = a.OriPrice;
          //bill.Discount = a.Discount;
          //bill.FinalPrice = Convert.ToString(Convert.ToInt32(a.FinalPrice) - card.Topped > 0 ? Convert.ToInt32(a.FinalPrice) - card.Topped : 0); //注意下相等情况
          ////注意Discount情况
          //bill.Comment = a.Comment;
          //bill.Time = DateTime.Now; //注入bill数据
          //_context.Bills.Add(bill);
          //bill.BillId = BillNumberInit;
          //_context.SaveChanges();//储存 获得bill主键id
          //CB.BillId = bill.Id;

          //int? ori_Topped = card.Topped; 
           
          //card.Topped = card.Topped - Convert.ToInt32(a.FinalPrice) > 0 ? card.Topped - Convert.ToInt32(a.FinalPrice) : 0;
          //_context.SaveChanges();

          CustomerProjectBill CPB = new CustomerProjectBill();
          CPB.CustomerId = a.CustomerId;
          CPB.ProjectId = a.ProjectId;
          CPB.BillId = idBill;
          CPB.ProjectNumber = a.ProjectNumber;
          //CPB.PaymentMethod = JsonSerializer.Serialize(a.PaymentMethods, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
          CPB.Staffid = a.StaffId;
          CPB.UnitPrice = a.UnitPrice;
          CPB.NetPrice = a.NetPrice;
          CPB.Discount = a.Discount;
          _context.CustomerProjectBills.Add(CPB);
         // _context.SaveChanges(); // 注入CustomerProjectBill 数据， 储存 获得其主键id
          customer = _context.Customers.Find(a.CustomerId);

          CustomerProject _CP = _context.CustomerProjects.Where(p => p.ProjectId == a.ProjectId && p.CustomerId == a.CustomerId).FirstOrDefault(); // 此处注意前方验证
          if (_CP == null)
          {
            CustomerProject cp = new CustomerProject(); //此处没有进行没有购买记录 传输为负数时的情况对应
            cp.CustomerId = a.CustomerId;
            cp.ProjectId = a.ProjectId;
            cp.Number = a.ProjectNumber;
            _context.CustomerProjects.Add(cp);
            _context.SaveChanges(); //此处完成了增加三表的数据的动作 用户购买Project的动作完成

            Cprecord _cpRecord = new Cprecord()
            {
              Time = DateTime.Now,
              Behaviour = cp.Number,
              CpId = cp.Id,
            };
            _context.Cprecords.Add(_cpRecord);
            //_context.SaveChanges();
          }
          else
          {
            _CP.Number += a.ProjectNumber;
            //_context.SaveChanges(); //检查是否已购买,已购买则直接增加/减少次数 前面已验证次数是否够

            Cprecord _cpRecord = new Cprecord()
            {
              Time = DateTime.Now,
              Behaviour = a.ProjectNumber,
              CpId = _CP.Id,
            };
              _context.Cprecords.Add(_cpRecord);
             // _context.SaveChanges();            
          }


          //开始记录账单
          Project project = _context.Projects.Find(a.ProjectId);
          Staff staff = _context.Staffs.Find(a.StaffId);
          cprdInvoices.Add(new InvoiceDto
          {
            ItemNo = project.ProjectName,
            Description = project.Introduction,
            UnitPrice = a.UnitPrice,
            NetPrice = a.NetPrice,
            Staff = staff.StaffName,
            StaffNumber = staff.StaffId,
            PaymentMethods = CPB.PaymentMethod,
          });
        

      }
      //增加账单 
      cprd.InvoiceNo = BillNumberInit;
      cprd.Date = DateTime.Now;
      cprd.CustomerNo = customer.CustomerId;
      cprd.CustomerName = customer.NameCn;
      cprd.Invoices = cprdInvoices;
      return cprd; //返还账单
    }
  }

}


//用于传输用户购买产品的账单
//POST api/<ProductController>
//[HttpPost("CustomerProductBill")]
//public IActionResult PostCustomerProductBill([FromBody] List<CustomerProductBillPostDto> value)
//{
//  if (value == null)
//  {
//    return BadRequest("传输的参数为空");
//  }
//  CustomerProductBillPostReturnDto cprd = new CustomerProductBillPostReturnDto();//用于第四部分返还账单

//  ProductClass productClass = new ProductClass();//获取产品种类信息
//  Customer customer = new Customer();//获取客户信息
//  Staff staff = new Staff();//或许操作的staff的信息
//  Product product = new Product();//用于随机获取产品


//  var productTable = _context.ProductClasses.Join(_context.ProductCategories,
//    productClass => productClass.Id, productCategory => productCategory.ClassId,
//    (productClass, productCategory) => new { productClass, productCategory }) //productClass 和 productCategory 根据 product Class 主键join
//  .Join(_context.Products, c => c.productCategory.ProductId, product => product.Id, (c, product) => new { c, product })
//  .Where(j => j.product.IfSelled == 0) //选中未被售卖产品
//  .Select(m => new
//  {
//    ProductName = m.c.productClass.ProductName,
//    ProductVolume = m.c.productClass.ProductVolume,
//    Introduction = m.c.productClass.Introduction,
//    pId = m.product.Id,
//    ProductId = m.c.productClass.ProductId,
//    ProductClassId = m.c.productClass.Id,
//  }).ToList(); //获取产品信息 获取product表中的信息

//  //三方面检查
//  foreach (var a in value)
//  {
//    productClass = _context.ProductClasses.Find(a.ProductId); //此处传输的是product class的id
//    customer = _context.Customers.Find(a.CustomerId);
//    staff = _context.Staffs.Find(a.StaffId);

//    if (productClass == null || customer == null || staff == null)
//    {
//      return BadRequest("产品、客户或员工信息缺失");
//    }

//    if (productTable.Where(i => i.ProductClassId == a.ProductId).FirstOrDefault() == null)
//    {
//      return BadRequest("产品库无产品信息 或产品无库存，请检查");
//    }

//    if (productClass.Storage == 0 || productClass.Storage == null || productClass.Storage < a.Number)
//    {
//      return BadRequest("产品无库存或库存不足，请检查");
//    }

//  }//check if has null value if has return fail

//  //创建List用于接受invoices
//  List<InvoiceDto> cprdInvoices = new List<InvoiceDto>();

//  //开始做billid 先获取最后一笔bill 如果没有 就为HG000000
//  string lastbillid = _context.Bills.OrderBy(e => e.Id).LastOrDefault().BillId ?? "HG000000";
//  int lastbillidnum = Convert.ToInt32(Regex.Replace(lastbillid, "[a-z]", "", RegexOptions.IgnoreCase));


//  //储存账单信息 出货 出库
//  foreach (var a in value)
//  {
//      Bill bill = new Bill();
//      //billid 更新规则 从HG000001开始
//      bill.OriPrice = a.OriPrice;
//      bill.Discount = a.Discount;
//      bill.FinalPrice = a.FinalPrice;
//      bill.Time = DateTime.Now;
//      bill.Comment = a.Comment; //进入loop循环，每次循环更新bill内容 并添加进相应表

//      _context.Bills.Add(bill);
//      bill.BillId = "HG" + string.Format("{0:D6}", lastbillidnum + 1);
//      _context.SaveChanges(); //保存 以此获得bill的id 传给CustomerProductBill

//      CustomerProductBill cpb = new CustomerProductBill();//用于第二部分记录账单
//      cpb.CustomerId = a.CustomerId;
//      cpb.ProductId = a.ProductId; //产品class的id
//      cpb.BillId = bill.Id;
//      cpb.Number = a.Number;

//      //遍历录入PaymentMethod信息 当string存
//      cpb.PaymentMethod = JsonSerializer.Serialize(a.PaymentMethods, new JsonSerializerOptions()
//      {
//        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
//      });
//      cpb.StaffId = a.StaffId;

//      _context.CustomerProductBills.Add(cpb);
//      _context.SaveChanges(); //保存 CustomerProductBill信息，完成任务，请考虑异步 同步

//      productClass = _context.ProductClasses.Find(a.ProductId);
//      customer = _context.Customers.Find(a.CustomerId);
//      staff = _context.Staffs.Find(a.StaffId);
//      product = _context.Products.Find(productTable.Where(i => i.ProductClassId == a.ProductId).FirstOrDefault().pId); //找相应产品种类id的产品 取第一个
//      product.IfSelled = 1;
//      productClass.Storage -= 1;
//      _context.SaveChanges();//完成出货 出库

//      cprdInvoices.Add(new InvoiceDto
//      {
//        ItemNo = productClass.ProductId,
//        Description = productClass.ProductName,
//        UnitPrice = a.OriPrice,
//        NetPrice = a.FinalPrice,
//        Staff = staff.StaffName,
//        StaffNumber = staff.StaffId,
//        PaymentMethods = cpb.PaymentMethod,
//      });
//  }
//  cprd.InvoiceNo = "HG" + string.Format("{0:D6}", lastbillidnum + 1);
//  cprd.Date = DateTime.Now;
//  cprd.CustomerNo = customer.CustomerId;
//  cprd.CustomerName = customer.NameCn;
//  cprd.Invoices = cprdInvoices;
//  return Ok(cprd);
//}

//用于购买项目次数和消费项目次数
//POST api/<ProductController>
//[HttpPost("CustomerProjectBill")]
//public IActionResult PostCustomerProjectBill([FromBody] List<CustomerProjectBillPostDto> value)
//{
//  if (value == null)
//  {
//    return BadRequest();
//  }

//  Customer customer = new Customer();//获取客户信息
//  CustomerProject CP = new CustomerProject();//获取项目次数
//  foreach (var a in value)
//  {
//    if (a.ProjectId == null || _context.Projects.Find(a.ProjectId) == null)
//    {
//      return BadRequest("请求的项目不存在");
//    };
//    customer = _context.Customers.Find(a.CustomerId);
//    if (a.CustomerId == null || customer == null)
//    {
//      return BadRequest("请求的客户不存在");
//    };
//    if (a.StaffId == null || _context.Staffs.Find(a.StaffId) == null)
//    {
//      return BadRequest("请求的员工不存在");
//    }; // 对每一项post数据进行检验
//    if (a.ProjectNumber == 0)
//    {
//      return BadRequest("传输的使用/增加产品数为0 不规范");
//    };
//    CP = _context.CustomerProjects.Where(p => p.ProjectId == a.ProjectId && p.CustomerId == a.CustomerId).FirstOrDefault(); // 验证次数是否够 //此处用于核销
//    if (a.ProjectNumber < 0 && CP != null && CP.Number < -a.ProjectNumber)
//    {
//      return BadRequest("用户拥有的项目次数不足，请先购买再请求");
//    }

//  }

//  CustomerProductBillPostReturnDto cprd = new CustomerProductBillPostReturnDto();
//  List<InvoiceDto> cprdInvoices = new List<InvoiceDto>(); // 用于记录账单

//  //开始做billid 先获取最后一笔bill 如果没有 就为HG000000
//  string lastbillid = _context.Bills.OrderBy(e => e.Id).LastOrDefault().BillId ?? "HG000000";
//  int lastbillidnum = Convert.ToInt32(Regex.Replace(lastbillid, "[a-z]", "", RegexOptions.IgnoreCase));

//  //开始插入数据 从Bills表开始 到CustomerProjectBills 到 CustomerProjects
//  foreach (var a in value)
//  {
//    Bill bill = new Bill();
//    //billid 更新规则 从HG000001开始
//    bill.OriPrice = a.OriPrice;
//    bill.Discount = a.Discount;
//    bill.FinalPrice = a.FinalPrice;
//    bill.Comment = a.Comment;
//    bill.Time = DateTime.Now; //注入bill数据
//    _context.Bills.Add(bill);
//    bill.BillId = "HG" + string.Format("{0:D6}", lastbillidnum + 1);
//    _context.SaveChanges();//储存 获得bill主键id

//    CustomerProjectBill CPB = new CustomerProjectBill();
//    CPB.CustomerId = a.CustomerId;
//    CPB.ProjectId = a.ProjectId;
//    CPB.BillId = bill.Id;
//    CPB.ProjectNumber = a.ProjectNumber;
//    CPB.PaymentMethod = JsonSerializer.Serialize(a.PaymentMethods, new JsonSerializerOptions()
//    {
//      Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
//    });
//    CPB.Staffid = a.StaffId;
//    _context.CustomerProjectBills.Add(CPB);
//    _context.SaveChanges(); // 注入CustomerProjectBill 数据， 储存 获得其主键id

//    CustomerProject _CP = _context.CustomerProjects.Where(p => p.ProjectId == a.ProjectId && p.CustomerId == a.CustomerId).FirstOrDefault(); // 此处注意前方验证
//    if (_CP == null)
//    {
//      CustomerProject cp = new CustomerProject(); //此处没有进行没有购买记录 传输为负数时的情况对应
//      cp.CustomerId = a.CustomerId;
//      cp.ProjectId = a.ProjectId;
//      cp.Number = a.ProjectNumber;
//      _context.CustomerProjects.Add(cp);
//      _context.SaveChanges(); //此处完成了增加三表的数据的动作 用户购买Project的动作完成
//    }
//    else
//    {
//      _CP.Number += a.ProjectNumber;
//      _context.SaveChanges(); //检查是否已购买,已购买则直接增加/减少次数 前面已验证次数是否够
//    }


//    //开始记录账单
//    Project project = _context.Projects.Find(a.ProjectId);
//    Staff staff = _context.Staffs.Find(a.StaffId);
//    cprdInvoices.Add(new InvoiceDto
//    {
//      ItemNo = project.ProjectName,
//      Description = project.Introduction,
//      UnitPrice = a.OriPrice,
//      NetPrice = a.FinalPrice,
//      Staff = staff.StaffName,
//      StaffNumber = staff.StaffId,
//      PaymentMethods = CPB.PaymentMethod,
//    });

//  }
//  //增加账单 
//  cprd.InvoiceNo = "HG" + string.Format("{0:D6}", lastbillidnum + 1);
//  cprd.Date = DateTime.Now;
//  cprd.CustomerNo = customer.CustomerId;
//  cprd.CustomerName = customer.NameCn;
//  cprd.Invoices = cprdInvoices;
//  return Ok(cprd); //返还账单
//}
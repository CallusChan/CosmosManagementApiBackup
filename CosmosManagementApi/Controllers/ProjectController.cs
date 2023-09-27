using AutoMapper;
using CosmosManagementApi.Dtos;
using CosmosManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using System.Xml.Schema;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosManagementApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProjectController : ControllerBase
  {
    private readonly ILogger<ProjectController> _logger;
    private readonly CosmosManagementDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;
    //private readonly AppSettingModel _appSettingModel;

    public ProjectController(ILogger<ProjectController> logger, CosmosManagementDbContext context, IMapper mapper, IWebHostEnvironment env)
    {
      _logger = logger;
      _context = context;
      _mapper = mapper;
      _env = env;
    }

    // GET: api/<ProductController>
    //获取所有项目信息
    [HttpGet]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult Get()
    {
      var result = _context.Projects.Where(p => p.IsDeleted == 0).ToList();
      var map = _mapper.Map<IEnumerable<ProjectGetDto>>(result);
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

    // GET api/<ProjectController>/5
    //获取单个项目信息
    [HttpGet("{id}")]
    [Authorize(Roles = "O1Staff, Admin")]

    public IActionResult Get(int id)
    {
      if (id == 0)
      {
        return BadRequest("Bad request");
      }
      var result = _context.Projects.Find(id);
      if (result == null)
      {
        return NotFound("未找到项目");
      }
      var map = _mapper.Map<ProjectGetDto>(result);
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

    //获取用户项目记录
    [HttpGet("CustomerProjectRecords/{id}")]
    [Authorize(Roles = "O1Staff, Admin")]

    public IActionResult GetCustomerPR(int id) 
    {
      var _table = _context.CustomerProjects.Where(m => m.CustomerId == id)
      .Join(_context.Cprecords, cp => cp.Id, cpr => cpr.CpId, (cp, cpr)=> new{cp, cpr})
      .Join(_context.Projects, cppr => cppr.cp.ProjectId, project => project.Id, (cppr, project)=>new { cppr,project})
      .Select(f => new
      {
        projectName = f.project.ProjectName,
        category = f.project.Category,
        bigCategory = f.project.BigCategory,
        projectNumber = f.cppr.cp.Number,
        projectBehaviour = f.cppr.cpr.Behaviour,
        Time = f.cppr.cpr.Time,
        pic_src = f.cppr.cpr.PicSrc,
      }).ToList();
      var r_json = new
      {
        total = _table.Count(),
        totalNotFiltered = _table.Count(),
        rows = _table
      };
      return new JsonResult(r_json) 
      {
        StatusCode = 200,
      };

    }

    //获取单个用户产品项目剩余次数
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpGet("GetCustomerProjectNumber/{id}")]
    public IActionResult GetCustomerProjectNumber(int id)
    {
      var table = _context.CustomerProjects
      .Join(_context.Projects, cp => cp.ProjectId, p => p.Id, (cp, p) => new { cp, p })
      .Where(j => j.cp.CustomerId == id && j.p.IsDeleted == 0)
      .Select(f => new
      {
        ProjectId = f.p.Id,
        ProjectName = f.p.ProjectName,
        ProjectNumber = f.cp.Number,
        Category = f.p.Category,
        BigCategory = f.p.BigCategory,
      }) ;

      var r_json = new
      {
        total = table.Count(),
        totalNotFiltered = table.Count(),
        rows = table
      };

      return new JsonResult(r_json) {  StatusCode = 200, };
      
    }

    //[HttpGet("ProjectType")]
    //public IActionResult GetProjectType()
    //{
    //  var r_json = new{ projectType =  };
    //}


    //获取项目种类
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpGet("GetCategories")]
    public IActionResult GetCategories()
    {
      var result = _context.ProjectCategories.Where(p => p.IsDeleted == 0).ToList();
      var map = _mapper.Map<IEnumerable<ProjectCategoriesGetDto>>(result);
      return Ok(map);
    }

    //获取充值後客⼈的消費選項
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpGet("GetBigCategories")]
    public IActionResult GetPurchaseCategories()
    {
      var result = _context.PurchaseCategories.Where(p => p.IsDeleted == 0).ToList();
      var map = _mapper.Map<IEnumerable<ProjectCategoriesGetDto>>(result);
      return Ok(map);
    }

    //传输新增项目种类
    [Authorize(Roles = "Admin")]
    [HttpPost("AddCategories")]
    public IActionResult PostCategories([FromBody] ProjectCategoriesPostDto value)
    {
      if(value == null){
        return BadRequest("传输值为零");
      }
      if (_context.ProjectCategories.Any(p => p.Category == value.Category)){
        return BadRequest("项目种类已存在");
      }
      var map = _mapper.Map<ProjectCategory>(value);
      map.IsDeleted = 0;
      map.WritingTime = DateTime.Now;
      _context.ProjectCategories.Add(map);
      _context.SaveChanges();
      return Ok(map);
    }

    //传输新增充值後客⼈的消費選項
    [Authorize(Roles = "Admin")]
    [HttpPost("AddBigCategories")]
    public IActionResult PostPurchaseCategories([FromBody] PurchaseCategoriesPostDto value)
    {
      if(value == null){
        return BadRequest("传输值为零");
      }
      if (_context.PurchaseCategories.Any(p => p.Category == value.Category)){
        return BadRequest("项目种类已存在");
      }
      var map = _mapper.Map<PurchaseCategory>(value);
      map.IsDeleted = 0;
      map.WritingTime = DateTime.Now;
      _context.PurchaseCategories.Add(map);
      _context.SaveChanges();
      return Ok(map);
    }


    // POST api/<ProductController>
    // 新增项目
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] ProjectAddDto value)
    {
      if(value == null){
        return BadRequest("传输项目为空");
      }
      if(value.ProjectTimes == null || value.ProjectTimes == 0){
        return BadRequest("传输参数不规范"); //传输的project拥有的次数需不为空
      }
      var map = _mapper.Map<Project>(value);
      map.IsDeleted = 0; //初创project 设为0
      _context.Projects.Add(map);
      _context.SaveChanges();
      return Ok(map);
    }

    //核销
    // POST api/<ProjectController>
    [HttpPost("WriteOff")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult PostVerification([FromBody] WriteOffDto value)
    {
      if (value == null)
      {
        return BadRequest("传输参数为空");
      }
      if (!_context.CustomerProjects.Any(p => p.CustomerId == value.Customer_id) 
      || !_context.CustomerProjects.Any(p => p.ProjectId == value.Project_id))
      {
        return BadRequest("此待核销记录不存在"); //传输的project拥有的次数需不为空
      }

      var rootPath = "C:\\Users\\Administrator\\Pictures\\PICS\\";
      //var rootPath = "C:\\Users\\Callus\\Pictures\\素材\\";

      var des = _context.CustomerProjects.Where(a => a.CustomerId == value.Customer_id && a.ProjectId == value.Project_id).First();
      Cprecord _cpRecord = new Cprecord();
      if (des.Number <= 0 ){
        return BadRequest("该顾客项目记录都已被核销");
      }else{
        if(value.Base_64 == null || value.Base_64.Length <= 0){
          return BadRequest("签名为空，请重传");
        }
        des.Number -= 1;
        _cpRecord.Time = DateTime.Now;
        _cpRecord.Behaviour = -1;
        _cpRecord.CpId = des.Id;
        _context.Cprecords.Add(_cpRecord);
         string format = "Mddyyyyhhmmsstt";
         string filePath = "CPrecordsCustomerId" + Convert.ToString(des.CustomerId) + "Time" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
         if(_context.Cprecords.Any(m => m.PicSrc == "http://43.155.94.131:8002/" + filePath))
         {
            return BadRequest("签名文件名已存在，请重传");
         }

         byte[] img_bytes = Convert.FromBase64String(value.Base_64);
         using (var stream = System.IO.File.Create(rootPath + filePath))
         {
           stream.Write(img_bytes);
         }
         _cpRecord.PicSrc = "http://43.155.94.131:8002/" + filePath;
      }
      _context.SaveChanges();
      var map = _context.Projects
      .Join(_context.CustomerProjects, project => project.Id, cp => cp.ProjectId, 
      (project, cp)=> new {project, cp}).Where(m => m.cp.CustomerId == value.Customer_id)
      .Join(_context.Cprecords, f => f.cp.Id, cpr => cpr.CpId, (f, cpr) => new { f, cpr })
      .Select(m => new{
        projectName = m.f.project.ProjectName,
        category = m.f.project.Category,
        time = m.cpr.Time,
        pic_src = m.cpr.PicSrc,
        number = m.f.cp.Number,
      });
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

    //核销上传签名
    // POST api/<ProjectController>
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpPost("Signature/{id}")]
    public IActionResult PostSignature([FromBody] string base64,int id)
    {
      var rootPath = "C:\\Users\\Administrator\\Pictures\\PICS\\";
      var _cp = _context.Cprecords.Find(id);
      if (_cp == null)
      {
        return BadRequest("该客户待核销记录不存在");
      }
      string filePath = "CPrecordsCustomerIdNiHaoCeshi" + Convert.ToString(id);
      if (base64.Length > 0)
      {


         if(_context.Cprecords.Any(m => m.PicSrc == "http://43.155.94.131:8002/" + filePath))
         {
            return BadRequest("图片名已存在，请重传");
         }
         
         byte[] img_bytes = Convert.FromBase64String(base64);
         using (var stream = System.IO.File.Create(rootPath + filePath))
         {
           stream.Write(img_bytes);
         }
      }
      _cp.PicSrc = "http://43.155.94.131:8002/" + filePath;
      _context.SaveChanges();
      return Ok();
    }


    // PUT api/<ProductController>/5
    //修改项目信息
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ProjectUpdateDto value)
    {
      if (value == null)
      {
        return BadRequest("传输参数为空");
      }
      if (value.ProjectTimes == null || value.ProjectTimes == 0)
      {
        return BadRequest("传输参数不规范"); //传输的project拥有的次数需不为空
      }

      var update = _context.Projects.Find(id);

      if (update == null || update.IsDeleted == 1)
      {
        return NotFound();
      }
      else
      {
        _mapper.Map(value, update); //AutoMapper 更新Project

        _context.SaveChanges(); //保存变更
      }

      return Ok(update);
    }

    // DELETE api/<ProjectController>/5
    //删除项目信息
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
      var delete = _context.Projects.Find(id);
      if (delete == null){
        return NotFound("request object not found");
      }
      delete.IsDeleted = 1;
      _context.SaveChanges();
      return Ok("项目记录已删除");
    }
  }
}

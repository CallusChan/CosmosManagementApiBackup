using AutoMapper;
using CosmosManagementApi.Dtos;
using CosmosManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Linq;
using Microsoft.Office.Interop.Word;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosManagementApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CustomerController : ControllerBase
  {
    private readonly ILogger<CustomerController> _logger;
    private readonly CosmosManagementDbContext _context;
    private readonly IMapper _mapper;

    public CustomerController(ILogger<CustomerController> logger, CosmosManagementDbContext context, IMapper mapper)
    {
      _logger = logger;
      _context = context;
      _mapper = mapper;
    } 

    // GET: api/<ValuesController>
    //获取全部会员信息
    [HttpGet]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult Get()
    {
      //var result = _context.Customers.Where(d => d.IsDeleted == 0).ToList();
      //var map = _mapper.Map<IEnumerable<CustomerGetDto>>(result);

      var result = (from a in _context.Customers
                    select new CustomerGetDto
                    {
                      Id = a.Id,
                      CustomerId = a.CustomerId,
                      NameCn = a.NameCn,
                      Rating = a.Rating,
                      Gender = a.Gender,
                      BirthdayDay  = a.Birthday == null ? null : a.Birthday.Value.Day,
                      BirthdayMonth = a.Birthday == null ? null : a.Birthday.Value.Month,
                      BirthdayYear = a.Birthday == null ? null : a.Birthday.Value.Year,
                      Phone = a.Phone,
                      Email = a.Email,
                      Location = a.Location,
                      RegDate = a.RegDate,
                      Level = a.Level,
                      Point = a.Point,
                      Img = a.Img,  
                      Comment = a.Comment,
                      KnoingMethod = a.KnoingMethod,
                      Age = a.Age,
                      Kind = a.Kind,
                    });

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

    // GET api/<ValuesController>/5
    //获取单个会员信息
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var result = _context.Customers.Find(id);
      if(result == null || result.IsDeleted == 1){
        return NotFound("无此顾客");
      }
      CustomerGetDto map = new CustomerGetDto
                    {
                      Id = result.Id,
                      CustomerId = result.CustomerId,
                      NameCn = result.NameCn,
                      Rating = result.Rating,
                      Gender = result.Gender,
                      BirthdayDay = result.Birthday == null ? null : result.Birthday.Value.Day,
                      BirthdayMonth = result.Birthday == null ? null : result.Birthday.Value.Month,
                      BirthdayYear = result.Birthday == null ? null :result.Birthday.Value.Year,
                      Phone = result.Phone,
                      Email = result.Email,
                      Location = result.Location,
                      RegDate = result.RegDate,
                      Level = result.Level,
                      Point = result.Point,
                      Img = result.Img,
                      Comment = result.Comment,
                      KnoingMethod = result.KnoingMethod,
                      Age = result.Age,
                      Kind = result.Kind,
                    };
      var projectTable = _context.Projects.Join(_context.CustomerProjects,
        project => project.Id, customerProject => customerProject.ProjectId,
        (project, customerProject) => new { project, customerProject })
        .Where(j => j.customerProject.CustomerId == id)
        .Select(m => new
        {
          ProjectName = m.project.ProjectName,
          ProjectTimes = m.customerProject.Number,
          Introduction = m.project.Introduction,
          Category = m.project.Category,
        }).ToList(); //创建对应表

      List<ProjectGetDto> pgd = new List<ProjectGetDto>();
      foreach (var a in projectTable){
        pgd.Add(new ProjectGetDto
        {
          ProjectName = a.ProjectName,
          ProjectTimes = a.ProjectTimes,
          Introduction = a.Introduction,
          Category = a.Category,
        });
      }

      map.projects = pgd;

      return Ok(map);
    }

    //获取客户的购买的产品
    //GET api/<ValuesController>/5
    [HttpGet("GetCustomerProductBills/{id}")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult GetCustomerProductBills(int id)
    {
      var result = _context.Bills.Join(_context.CustomerProductBills,
      bill => bill.Id, cpb => cpb.BillId, (bill, cpb) => new { bill, cpb }).Where(i => i.cpb.CustomerId == id)
      .Join(_context.Products, productbill => productbill.cpb.ProductId, product => product.Id, (productbill, product) => new { productbill, product })
      .Join(_context.ProductCategories, ppb => ppb.product.Id, category => category.ProductId, (ppbc, category) => new { ppbc, category })
      .Join(_context.ProductClasses, d => d.category.ClassId, cass => cass.Id, (ppbcc, cass) => //join 两表 获取数据
      new CustomerProductBillGetDto
      {
        BillId = ppbcc.ppbc.productbill.bill.BillId,
        OriPrice = ppbcc.ppbc.productbill.bill.OriPrice,
        Discount = ppbcc.ppbc.productbill.bill.Discount,
        FinalPrice = ppbcc.ppbc.productbill.bill.FinalPrice,
        Time = ppbcc.ppbc.productbill.bill.Time,
        Comment = ppbcc.ppbc.productbill.bill.Comment,
        Number = ppbcc.ppbc.productbill.cpb.Number,
        PaymentMethod = ppbcc.ppbc.productbill.cpb.PaymentMethod,
        StaffId = ppbcc.ppbc.productbill.cpb.StaffId,
        ProductId = ppbcc.ppbc.product.Id,
        Product = _mapper.Map<ProductClassDto>(cass),
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

    //获取客户的购买的项目
    //GET api/<ValuesController>/5
    [HttpGet("GetCustomerProjectBills/{id}")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult GetCustomerProjectBills(int id)
    {
      var result = _context.Bills.Join(_context.CustomerProjectBills,
      bill => bill.Id, cpb => cpb.BillId, (bill, cpb) => new { bill, cpb }).Where(i => i.cpb.CustomerId == id)
      .Join(_context.Projects, c => c.cpb.ProjectId, project => project.Id, (c, project) => //join 两表 获取数据
      new CustomerProjectBillGetDto
      {
        BillId = c.bill.BillId,
        OriPrice = c.bill.OriPrice,
        Discount = c.bill.Discount,
        FinalPrice = c.bill.FinalPrice,
        Time = c.bill.Time,
        Comment = c.bill.Comment,
        ProjectNumber = c.cpb.ProjectNumber,
        PaymentMethod = c.cpb.PaymentMethod,
        StaffId = c.cpb.Staffid,
        ProjectId = project.Id,
        Project = project,
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

    //获取一年内生日的用户
    // GET: api/<ValuesController>
    [HttpGet("Birthday/year")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult GetCustomerBirthDAYInYear()
    {
      

      var result = (from a in _context.Customers
                    where a.Birthday.Value.Year == DateTime.Now.Year
                    select new CustomerGetDto
                    {
                      Id = a.Id,
                      CustomerId = a.CustomerId,
                      NameCn = a.NameCn,
                      Rating = a.Rating,
                      Gender = a.Gender,
                      BirthdayDay = a.Birthday.Value.Day,
                      BirthdayMonth = a.Birthday.Value.Month,
                      BirthdayYear = a.Birthday.Value.Year,
                      Phone = a.Phone,
                      Email = a.Email,
                      Location = a.Location,
                      RegDate = a.RegDate,
                      Level = a.Level,
                      Point = a.Point,
                      Img = a.Img,
                      Comment = a.Comment,
                      KnoingMethod = a.KnoingMethod,
                      Age = a.Age,
                      Kind = a.Kind,
                    }).ToList();

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

    //获取一月内生日的用户
    // GET: api/<ValuesController>
    [HttpGet("Birthday/month")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult GetCustomerBirthDAYInMonth()
    {


      var result = (from a in _context.Customers
                    where a.Birthday.Value.Month == DateTime.Now.Month
                    select new CustomerGetDto
                    {
                      Id = a.Id,
                      CustomerId = a.CustomerId,
                      NameCn = a.NameCn,
                      Rating = a.Rating,
                      Gender = a.Gender,
                      BirthdayDay = a.Birthday.Value.Day,
                      BirthdayMonth = a.Birthday.Value.Month,
                      BirthdayYear = a.Birthday.Value.Year,
                      Phone = a.Phone,
                      Email = a.Email,
                      Location = a.Location,
                      RegDate = a.RegDate,
                      Level = a.Level,
                      Point = a.Point,
                      Img = a.Img,
                      Comment = a.Comment,
                      KnoingMethod = a.KnoingMethod,
                      Age = a.Age,
                      Kind = a.Kind,
                    }).ToList();

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

    //获取一星期内生日的用户
    // GET: api/<ValuesController>
    [HttpGet("Birthday/week")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult GetCustomerBirthDAYInWeek()
    {
      //DateTime endDate = DateTime.Now + new TimeSpan(4, 12, 0, 0) + new TimeSpan(2, 12, 0, 0);

      //DateTime 
      int todayDate = DateTime.Today.Day;
      int todayMonth = DateTime.Today.Month;
      DateTime startDate = new DateTime(1993, todayMonth, todayDate);
      DateTime endDate = startDate.AddDays(7);

      var result = (from a in _context.Customers
                    select new CustomerGetDto
                    {
                      Id = a.Id,
                      CustomerId = a.CustomerId,
                      NameCn = a.NameCn,
                      Rating = a.Rating,
                      Gender = a.Gender,
                      BirthdayDay = a.Birthday.Value.Day,
                      BirthdayMonth = a.Birthday.Value.Month,
                      BirthdayYear = a.Birthday.Value.Year,
                      Phone = a.Phone,
                      Email = a.Email,
                      Location = a.Location,
                      RegDate = a.RegDate,
                      Level = a.Level,
                      Point = a.Point,
                      Img = a.Img,
                      Comment = a.Comment,
                      KnoingMethod = a.KnoingMethod,
                      Age = a.Age,
                      Kind = a.Kind,
                    }).ToList();
      var return_list = new List<CustomerGetDto>();
      DateTime c_birthday = new DateTime(1993, 1, 1);
        foreach(var a in result){
            if(a.BirthdayDay == null){
                continue;
            }
            if(a.BirthdayMonth == null){
                continue;
            }
            int cBrithDayMonth = a.BirthdayMonth.Value;
            int cBrithDayDay = a.BirthdayDay.Value;
            c_birthday = new DateTime(1993, cBrithDayMonth, cBrithDayDay);
            if (c_birthday >= startDate && c_birthday <= endDate){
              return_list.Add(a);
            }
        }
      var r_json = new
      {
        total = result.Count(),
        totalNotFiltered = result.Count(),
        rows = return_list
      };
      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }

    // POST api/<ValuesController>
    //新增会员
    [HttpPost]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult Post([FromBody] CustomerAddDto value)
    {
      if (value == null)
      {
        return BadRequest("Vaild Input");
      }
      var map = _mapper.Map<Customer>(value);
      map.RegDate = DateTime.Today; //Reg-date is tadays date
      map.IsDeleted = 0; //At created Customer is not deleted
      map.Point = 0; // initial point is 0
      map.Level = "iron"; // lowerest level inital
      map.Birthday = new DateTime(value.BirthdayYear, value.BirthdayMonth, value.BirthdayDay);

      _context.Customers.Add(map);
      _context.SaveChanges(); 
      return Ok(map);
    }

    // PUT api/<ValuesController>/5
    //修改会员信息
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Put(int id, [FromBody] CustomerUpdateDto value)
    {
      var update = _context.Customers.Find(id);
      if (update == null || update.IsDeleted == 1) 
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

    // DELETE api/<ValuesController>/5
    //删除用户
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
      var delete = _context.Customers.Find(id);
      if(delete == null || delete.IsDeleted == 1){
        return NotFound("已被删除或记录不存在");
      }
      delete.IsDeleted = 1;
      _context.SaveChanges(); //不进行直接删除 只将is deleted 设置为1 表示已经删除
      return Ok("用户已删除");
    }

    private bool isBirthWeek(DateTime birthDate)
    {
       DateTime dtStart = DateTime.Today;

            // Find the previous monday
       while (dtStart.DayOfWeek != DayOfWeek.Monday)
       {
          dtStart = dtStart.AddDays(-1);
       }

       DateTime dtEnd = dtStart.AddDays(7);

       int dayStart = dtStart.DayOfYear;
       int dayEnd = dtEnd.DayOfYear;
       int birthDay = birthDate.DayOfYear;
       return (birthDay >= dayStart && birthDay < dayEnd);

    }

  }
}

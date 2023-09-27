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
using BCrypt.Net;
using Azure.Core;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosManagementApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StaffController : ControllerBase
  {
    private readonly ILogger<StaffController> _logger;
    private readonly CosmosManagementDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public StaffController(ILogger<StaffController> logger, CosmosManagementDbContext context, IMapper mapper, IConfiguration configuration)
    {
      _logger = logger;
      _context = context;
      _mapper = mapper;
      _configuration = configuration;
    } 

    // GET: api/<ValuesController>
    [HttpGet]
    public IActionResult Get()
    { 
      var result = _context.Staffs.ToList();
      var map = _mapper.Map<IEnumerable<StaffGetDto>>(result);
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

    [HttpGet("NoLogin")] //此处做未登录是的api跳转
    public string noLogin()
    {
      return "未登入";
    }

    // GET api/<ValuesController>/5
    //获取单个staff的信息
    [HttpGet("{id}")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult Get(int id)
    {
      var result = _context.Staffs.Find(id);
      if(result == null){
        return NotFound();
      }
      var map = _mapper.Map<StaffGetDto>(result);
      return Ok(map);
    }

    // POST api/<ValuesController>
    [HttpPost]
    //增加新员工
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] StaffAddDto value)
    {
      if (value == null)
      {
        return BadRequest("Vaild Input");
      }

      if (_context.StaffAccounts.Any(x => x.AccountName == value.AccountName))
      {
        return BadRequest("Account Name already exist");
      }

      var map = _mapper.Map<Staff>(value);
      map.OnboardDate = DateTime.Today; //Reg-date is tadays date
      map.IfOnboard = 1; //At created it is 1


      _context.Staffs.Add(map);
      _context.SaveChanges(); // 保存更改 获得staff的id primary key


      var account = new StaffAccountCreateDto
      {
        AccountId = "Deafault ID",
        AccountName = value.AccountName,
        Pwd = value.Pwd,
        //PwdHash = BCrypt.Net.BCrypt.HashPassword(value.Pwd),
        //RegTime = DateTime.UtcNow(),
        //LastLoginTime
        //LastLoginIP
        //LogInFailedTime
        Level = value.Level,
        StaffId = map.Id,
      };

      var mapAccount = _mapper.Map<StaffAccount>(account);

      mapAccount.PwdHash = BCrypt.Net.BCrypt.HashPassword(value.Pwd);
      mapAccount.RegTime = DateTime.UtcNow;
      mapAccount.LastLogInTime = DateTime.UtcNow;
      mapAccount.LsatLogInIp = Convert.ToString(Request.HttpContext.Connection.RemoteIpAddress);
      mapAccount.LastLogInTime = null; //设定默认属性 在新建用户时同时新建账户
      mapAccount.IsLock = 0;
      mapAccount.IsActive = 1;
      mapAccount.LogInFailedTime = null;
      mapAccount.LogInFailedTimes = 0;

      _context.StaffAccounts.Add(mapAccount);
      _context.SaveChanges();

      var r_json = new
      {
        Staff = map,
        StaffAccount = mapAccount,
      };
      return new JsonResult(r_json)
      {
        StatusCode = 200,
      };
    }

    [HttpPost("Login")]
    public IActionResult Login(StaffLoginDto value)
    {
      var user = (from a in _context.StaffAccounts
                  where a.AccountName == value.AccountName
                  && a.Pwd == value.Pwd
                  select a).SingleOrDefault();

      if (user == null)
      {
        return BadRequest("Account Not Found");
      }
      else
      {
        var claims = new List<Claim>
        {
                    new Claim(ClaimTypes.Name, user.AccountName),
                    new Claim("FullName", user.AccountId),
                    //new Claim(ClaimTypes.Role, Convert.ToString(user.Level)),
        };

        var authProperties = new AuthenticationProperties
        {
          //AllowRefresh = <bool>,
          // Refreshing the authentication session should be allowed.

          //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
          // The time at which the authentication ticket expires. A 
          // value set here overrides the ExpireTimeSpan option of 
          // CookieAuthenticationOptions set with AddCookie.

          //IsPersistent = true,
          // Whether the authentication session is persisted across 
          // multiple requests. When used with cookies, controls
          // whether the cookie's lifetime is absolute (matching the
          // lifetime of the authentication ticket) or session-based.

          //IssuedUtc = <DateTimeOffset>,
          // The time at which the authentication ticket was issued.

          //RedirectUri = <string>
          // The full path or absolute URI to be used as an http 
          // redirect response value.
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        return Ok("登陆成功");
      }
    }

    [HttpPost("LoginJwt")]
    public ActionResult<Staff> LoginJwt (StaffLoginDto request)
    {
            var user = (from a in _context.StaffAccounts
                        where a.AccountName == request.AccountName
                        select a).SingleOrDefault();

            if (user == null)
            {
                return BadRequest("Account Not Found");
            }
            
            if(!BCrypt.Net.BCrypt.Verify(request.Pwd, user.PwdHash))
            {
                return BadRequest("Wrong Password 密码错误");
            }

            string token = CreateToken(request);

            return Ok(token);
    }

    // PUT api/<ValuesController>/5
    //修改员工信息
    [HttpPut("{id}")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult Put(int id, [FromBody] StaffUpdateDto value)
    {
      var update = _context.Staffs.Find(id);
      if (_context.StaffAccounts.Any(x => x.AccountName == value.AccountName))
      {
        return BadRequest("Account Name already exist");
      }
      if (update == null) 
      {
        return NotFound(); 
      }
      else
      {
        _mapper.Map(value, update); //AutoMapper 更新Customer

        _context.SaveChanges(); //保存变更

        var account = _context.StaffAccounts.Where(m => m.StaffId == id).First();
        account.AccountId = "Deafault ID";
        account.AccountName = value.AccountName;
        account.Pwd = value.Pwd;
        account.PwdHash = BCrypt.Net.BCrypt.HashPassword(value.Pwd);
        account.Level = value.Level;
        _context.SaveChanges();
        };
      
      return Ok(update);
    }

    // DELETE api/<ValuesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {

    }

    [HttpDelete("LogOut")]
    public void logout()
    {
      HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private string CreateToken(StaffLoginDto account)
    { 
        var _account = _context.StaffAccounts.Where(m => m.AccountName == account.AccountName && m.Pwd == account.Pwd).FirstOrDefault();
        List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.Name, account.AccountName),
        }; 
        if (_account.Level == 1 || _account == null)
        {
            claims.Add( new Claim(ClaimTypes.Role, "O1Staff"));
        }else if (_account.Level == 2) {
            claims.Add( new Claim(ClaimTypes.Role, "O2Staff"));
        }else if( _account.Level == 3) {
            claims.Add( new Claim(ClaimTypes.Role, "O3Staff"));
        }else if(_account.Level == 4) {
            claims.Add( new Claim(ClaimTypes.Role, "O4Staff"));
        }else if ( _account.Level == 5) {
            claims.Add( new Claim(ClaimTypes.Role, "Admin"));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("JwtKey:Token").Value!));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),//two hours will expire
                signingCredentials:creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
  }
}

using AutoMapper;
using CosmosManagementApi.Dtos;
using CosmosManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace CosmosManagementApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CardController : ControllerBase
  {
    private readonly ILogger<CardController> _logger;
    private readonly CosmosManagementDbContext _context;
    private readonly IMapper _mapper;
    public CardController(ILogger<CardController> logger, CosmosManagementDbContext context, IMapper mapper)
    {
      _logger = logger;
      _context = context;
      _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Get() 
    {
      var cardTable = _context.Cards.Join(_context.Customers,
        card => card.CustomerId, customer => customer.Id, 
        (card, customer) => new {card, customer})
      .Select(m => new
      {
        CardId = m.card.Id,
        CardNo = m.card.CardNo,
        Topped = m.card.Topped,
        CustomerName = m.customer.NameCn,
        Comment = m.customer.Comment,
        Age = m.customer.Age
      }).ToList();

      return Ok(cardTable);
    }

    //获取单个会员card和会员信息 
    [HttpGet("{id}")]
    [Authorize(Roles = "O1Staff, Admin")]
    public IActionResult Get(int id)
    {
      if(!_context.Customers.Any(c => c.Id == id))
      {
        return BadRequest("用户不存在");
      }
      var cardTable = _context.Cards.Join(_context.Customers,
        card => card.CustomerId, customer => customer.Id,
        (card, customer) => new { card, customer })
      .Where(c => c.customer.Id == id)
      .Select(m => new
      {
        CardId = m.card.Id,
        CardNo = m.card.CardNo,
        Topped = m.card.Topped,
        CustomerName = m.customer.NameCn,
        Comment = m.customer.Comment,
        Age = m.customer.Age
      }).ToList();



      return Ok(cardTable);
    }

    //获取card 
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpGet("CustomerCard/{id}")]
    public IActionResult GetCusteomrCard(int id)
    {
      if (!_context.Customers.Any(c => c.Id == id))
      {
        return BadRequest("用户不存在");
      }
      if (_context.Cards.Where(c => c.CustomerId == id && c.Topped != 0).IsNullOrEmpty())
      {
        var cardNullTable = new
        {
            CardId = 999999,
            CardNo = "不存在",
            Topped = 0,
            Discount = "D",
        };
        return Ok(cardNullTable);
      }

      var cardTable = _context.Cards.Where(c => c.CustomerId == id && c.Topped != 0)
      .Select(m => new
      {
        CardId = m.Id,
        CardNo = m.CardNo,
        Topped = m.Topped,
        Discount = m.CardNo == "S"?0.4 : m.CardNo =="A"? 0.6: m.CardNo == "B"?0.7 : m.CardNo == "C"?0.8: 1.0,
      }).ToList();

      return Ok(cardTable);
    }

    //添加新卡
    [Authorize(Roles = "O1Staff, Admin")]
    [HttpPost("AddNewCard")]
    public IActionResult PostAddNewCard([FromBody] CardAddDto value)
    {
      if (value == null)
      {
        return BadRequest("传输的参数为空");
      }
      //验证员工
      //if (!_context.Staffs.Any(staff => staff.Id == value.StaffId))
      //{
      //  return BadRequest("员工不存在,请核实");
      //}
      if (!_context.Customers.Any(customer => customer.Id == value.CustomerId))
      {
        return BadRequest("员工不存在,请核实");
      }


      Card card = new Card
      {
        CustomerId = value.CustomerId,
        CardNo = value.CardNo,
        Topped = value.Topped,
      };
      _context.Add(card);
      _context.SaveChanges();

      string lastbillid = _context.Bills.OrderBy(e => e.Id).LastOrDefault().BillId ?? "HG000000";
      int lastbillidnum = Convert.ToInt32(Regex.Replace(lastbillid, "[a-z]", "", RegexOptions.IgnoreCase));

      Bill bill = new Bill
      {
        BillId = "HG" + string.Format("{0:D6}", lastbillidnum + 1),
        OriPrice = Convert.ToString(value.Topped),
        Discount = "会员卡",
        FinalPrice = Convert.ToString(value.Topped),
        Time = DateTime.Now,
        Comment = value.Comment,
      };
      _context.Add(bill);
      _context.SaveChanges();

      CardBill cardBill = new CardBill
      {
        CardId = card.Id,
        BillId = bill.Id,
        PaymentMethod = JsonSerializer.Serialize(value.PaymentMethods, new JsonSerializerOptions()
        {
          Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        }),
        StaffId = 99999,
      };
      _context.Add(cardBill);
      _context.SaveChanges();

      Customer customer = _context.Customers.Find(value.CustomerId);

      CardBillReturnDto crd = new CardBillReturnDto
      {
        CustomerName = customer.NameCn,
        CardNo = value.CardNo,
        Comment = value.Comment,
        Date = DateTime.Now,
        PaymentMethods = cardBill.PaymentMethod,
        Topped = Convert.ToString(value.Topped),
      }; 

      return Ok(crd); 

    }

    //卡消费
    //[HttpPost("PayByCard")]
    //public IActionResult PayByCard([FromBody] CardPayDto value)
    //{
    //  var 
    //}
  }
}

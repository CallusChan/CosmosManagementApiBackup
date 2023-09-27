using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CardBillReturnDto
  {

    /// <summary>
    /// Card number
    /// </summary>
    public string? CardNo { get; set; }

    /// <summary>
    /// Card top-up value
    /// </summary>
    public string? Topped { get; set; }

    //Payment methods
    public string? PaymentMethods { get; set; }

    public string? Comment { get; set; }
    public string? CustomerNo { get; set;}
    public string? CustomerName { get; set; }
    public DateTime? Date { get; set; }
  }
}

using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CardAddDto
  {
    /// <summary>
    /// ReferenceKey
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Card number
    /// </summary>
    public string? CardNo { get; set; }

    /// <summary>
    /// Card top-up value
    /// </summary>
    public int? Topped { get; set; }

    //Payment methods
    public List<PaymentMethodDto>? PaymentMethods { get; set; }

    public string? Comment { get; set; }
  }
}

using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CardDto
  {
    /// <summary>
    /// Primary key for Customer Card
    /// </summary>
    public int Id { get; set; }

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
    public string? Topped { get; set; }

  }
}

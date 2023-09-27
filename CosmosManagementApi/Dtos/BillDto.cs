using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class BillDto
  {
    /// <summary>
    /// Primary key for bills
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bill id in company
    /// </summary>
    public string? BillId { get; set; }

    /// <summary>
    /// Original price
    /// </summary>
    public string? OriPrice { get; set; }

    /// <summary>
    /// Discount for this bill
    /// </summary>
    public string? Discount { get; set; }

    /// <summary>
    /// Final price of bill
    /// </summary>
    public string? FinalPrice { get; set; }

    /// <summary>
    /// When made
    /// </summary>
    public DateTime? Time { get; set; }

    /// <summary>
    /// Comments
    /// </summary>
    public string? Comment { get; set; }
  }
}

using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CustomerProductBillGetDto
  {

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

    /// <summary>
    /// Customer id
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Product id
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// How many product bought
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// how is it paid
    /// </summary>
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Staff who made this purchase
    /// </summary>
    public int? StaffId { get; set; }

    public string? StaffName{ get; set; }

    public ProductClassDto? Product { get; set; }
  }
}

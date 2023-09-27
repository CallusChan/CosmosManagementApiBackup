using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CustomerProjectBillPostDto
  {

    /// <summary>
    /// Original price
    /// </summary>
    public string? UnitPrice { get; set; }


    /// <summary>
    /// Final price of bill
    /// </summary>
    public string? NetPrice { get; set; }

    public string? Discount{ get; set; }

    /// <summary>
    /// Comments
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Customer id
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Project id
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// How many product bought
    /// </summary>
    public int? ProjectNumber { get; set; }

    /// <summary>
    /// how is it paid
    /// </summary>
    //public List<PaymentMethodDto>? PaymentMethods { get; set; }

    /// <summary>
    /// Staff who made this purchase
    /// </summary>
    public int? StaffId { get; set; }
  }
}

using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CustomerProjectBillGetDto
  {

    public string? BillId { get; set; }


    public string? OriPrice { get; set; }

    public string? ProjectName { get; set; }
    public string? Discount { get; set; }

    public string? FinalPrice { get; set; }

    public DateTime? Time { get; set; }

    public string? Comment { get; set; }

    public int CustomerId { get; set; }

    public int ProjectId { get; set; }

    public int? ProjectNumber { get; set; }

    public string? PaymentMethod { get; set; }
    public int? StaffId { get; set; }

    public string? StaffName { get; set; }

    public string? CommentOfProjectLine { get; set; }

    public Project? Project { get; set; }
  }
}

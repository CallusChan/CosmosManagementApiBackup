using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class InvoiceDto
  {
    public string? ItemNo{ get; set; }
    public string? Description { get; set; }
    public string? UnitPrice  { get; set; }
    public string? NetPrice { get; set; }
    public string? Staff  { get; set; }
    public string? StaffNumber{ get; set; }
    public string? PaymentMethods{ get; set; }
  }
}

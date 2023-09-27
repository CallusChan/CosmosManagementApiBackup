using CosmosManagementApi.Models;
using CosmosManagementApi.Dtos;

namespace CosmosManagementApi.Dtos
{
  public class CustomerBillPostReturnDto
  {

    public string? InvoiceNo { get; set; }
    public DateTime? Date { get; set; }
    public string? CustomerNo { get; set; }
    public string? CustomerName { get; set; }

    public List<InvoiceDto>? Invoices { get; set; }
  }
}

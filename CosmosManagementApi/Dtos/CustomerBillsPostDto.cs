using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CustomerBillsPostDto
  {
    public List<CustomerProductBillPostDto>? ProductBills { get; set; }

    public List<CustomerProjectBillPostDto>? ProjectBills { get; set; }

    public List<PaymentMethodDto>? PaymentMethods { get; set; }

    public string? OriPrice { get; set; }

    public string? Discount { get; set; }

    public string? FinalPrice { get; set; }

    public string? Comment { get; set; }

    public int? CustomerId{ get; set; }
    
  }
}

using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CustomerBillsReturnDto
  {
    public CustomerProductBillPostReturnDto? ProductBillsReturn { get; set; }

    public CustomerProjectBillPostReturnDto? ProjectBillsReturn { get; set; }
    
  }
}

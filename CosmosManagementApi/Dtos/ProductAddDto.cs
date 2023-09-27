using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class ProductAddDto
  {
    public DateTime? ProductDate { get; set; }
    public DateTime? ProductEndDate { get; set; }
    public int? Number{ get; set; }
    public int? ClassId { get; set; }
    public string? Price { get; set; }

  }
}

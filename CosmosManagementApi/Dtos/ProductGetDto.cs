using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class ProductGetDto
  {
    /// <summary>
    /// Primary key for product
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product purchse date
    /// </summary>
    public DateTime? ProductDate { get; set; }

    /// <summary>
    /// Product end date
    /// </summary>
    public DateTime? ProductEndDate { get; set; }

    /// <summary>
    /// If product is selled
    /// </summary>
    public int? IfSelled { get; set; }

    public int? CategoryId { get; set; }

  }
}

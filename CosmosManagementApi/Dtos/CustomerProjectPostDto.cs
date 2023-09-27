using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CustomerProjectPostDto
  {

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
    public int ProjectId { get; set; }

    /// <summary>
    /// How many projects consumed bought
    /// </summary>
    public int? Number { get; set; }

    public int? StaffId { get; set; }
  }
}

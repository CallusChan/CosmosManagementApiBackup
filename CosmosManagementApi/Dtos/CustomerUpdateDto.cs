using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class CustomerUpdateDto
  {
    /// <summary>
    /// Customer number in systen
    /// </summary>
    public string? CustomerId { get; set; }

    /// <summary>
    /// Customer chinese name
    /// </summary>
    public string? NameCn { get; set; }

    /// <summary>
    /// Customer english name
    /// </summary>
    public string? Rating { get; set; }

    /// <summary>
    /// Customer gender
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Customer birthday
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Cusomer Phone number
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Customer Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Customer living location
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// When customer registered in system
    /// </summary>
    public DateTime? RegDate { get; set; }

    /// <summary>
    /// Customer Level
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// Customer points
    /// </summary>
    public int? Point { get; set; }

    /// <summary>
    /// path to customer img
    /// </summary>
    public string? Img { get; set; }

    /// <summary>
    /// Special comment of the user
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// How Knows This Company
    /// </summary>
    public string? KnoingMethod { get; set; }

    /// <summary>
    /// User Age in brief
    /// </summary>
    public string? Age { get; set; }

    public string? Kind { get; set; }

  }
}

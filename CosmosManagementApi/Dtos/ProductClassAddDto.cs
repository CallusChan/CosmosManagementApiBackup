using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class ProductClassAddDto
  {
    /// <summary>
    /// Product Id
    /// </summary>
    public string? ProductId { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Product size volume
    /// </summary>
    public string? ProductVolume { get; set; }

    /// <summary>
    /// Product purchse date
    /// </summary>
    public DateTime? ProductDate { get; set; }

    /// <summary>
    /// Product end date
    /// </summary>
    public DateTime? ProductEndDate { get; set; }

    /// <summary>
    /// Product introduction
    /// </summary>
    public string? Introduction { get; set; }

    /// <summary>
    /// Path to img directory
    /// </summary>
    public string? Img { get; set; }

    /// <summary>
    /// Buying price of product
    /// </summary>
    public string? BuyingPrice { get; set; }

    /// <summary>
    /// Recommened selling price
    /// </summary>
    public string? SellingPrice { get; set; }

    public string? Category { get; set; }


    public int? Storage{ get; set; }


  }
}

﻿using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class ProductClassUpdateDto
  {

    /// <summary>
    /// Product name
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Product size volume
    /// </summary>
    public string? ProductVolume { get; set; }

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

    /// <summary>
    /// ProductClassCategory
    /// </summary>
    public string? Category { get; set; }
    public string? BigCategory { get; set; }



  }
}

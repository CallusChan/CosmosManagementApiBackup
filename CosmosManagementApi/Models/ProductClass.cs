using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class ProductClass
{
    /// <summary>
    /// Primary key for product
    /// </summary>
    public int Id { get; set; }

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

    /// <summary>
    /// Product Company id
    /// </summary>
    public string? ProductId { get; set; }

    /// <summary>
    /// How many item in storage
    /// </summary>
    public int? Storage { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();
}

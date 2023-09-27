using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class ProductCategory
{
    /// <summary>
    /// Primary key for Product Category
    /// </summary>
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? ClassId { get; set; }

    public DateTime? InStorageTime { get; set; }

    public virtual ProductClass? Class { get; set; }

    public virtual Product? Product { get; set; }
}

using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class Product
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

    public virtual ICollection<CustomerProductBill> CustomerProductBills { get; } = new List<CustomerProductBill>();

    public virtual ICollection<ProductCategory> ProductCategories { get; } = new List<ProductCategory>();

    public virtual ICollection<ProductStorageBill> ProductStorageBills { get; } = new List<ProductStorageBill>();

    public virtual ICollection<StaffProductBill> StaffProductBills { get; } = new List<StaffProductBill>();
}

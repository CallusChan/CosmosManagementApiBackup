using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class PurchaseCategory
{
    /// <summary>
    /// PrimaryKey
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Purchase category after topup
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// when record
    /// </summary>
    public DateTime? WritingTime { get; set; }

    /// <summary>
    /// 1 for deleted
    /// </summary>
    public int? IsDeleted { get; set; }
}

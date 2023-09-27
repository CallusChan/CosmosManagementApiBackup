using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class Card
{
    /// <summary>
    /// Primary key for Customer Card
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ReferenceKey
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Card number
    /// </summary>
    public string? CardNo { get; set; }

    public decimal? Topped { get; set; }

    public virtual ICollection<CardBill> CardBills { get; } = new List<CardBill>();

    public virtual Customer? Customer { get; set; }
}

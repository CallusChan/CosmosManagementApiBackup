using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class StaffProductBill
{
    /// <summary>
    /// Primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Customer id
    /// </summary>
    public int StaffId { get; set; }

    /// <summary>
    /// Product id
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Bill id
    /// </summary>
    public int BillId { get; set; }

    /// <summary>
    /// How many product bought
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// how is it paid
    /// </summary>
    public string? PaymentMethod { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}

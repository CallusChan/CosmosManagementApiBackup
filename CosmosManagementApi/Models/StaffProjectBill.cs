using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class StaffProjectBill
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
    public int ProjectId { get; set; }

    /// <summary>
    /// Bill id
    /// </summary>
    public int BillId { get; set; }

    /// <summary>
    /// How many times project will do
    /// </summary>
    public int? ProjectNumber { get; set; }

    /// <summary>
    /// how is it paid
    /// </summary>
    public string? PaymentMethod { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}

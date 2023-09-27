using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class CustomerProjectBill
{
    /// <summary>
    /// Primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Customer id
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Product id
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Bill id
    /// </summary>
    public int BillId { get; set; }

    /// <summary>
    /// How many times the Project will go through
    /// </summary>
    public int? ProjectNumber { get; set; }

    /// <summary>
    /// how is it paid
    /// </summary>
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Staff who made this purchase
    /// </summary>
    public int? Staffid { get; set; }

    /// <summary>
    /// Unit price for a project
    /// </summary>
    public string? UnitPrice { get; set; }

    /// <summary>
    /// Sum of unit price
    /// </summary>
    public string? NetPrice { get; set; }

    /// <summary>
    /// Discount used to calculate net price
    /// </summary>
    public string? Discount { get; set; }

    /// <summary>
    /// Comment of this bill
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// which category this bill belongs to
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// The biggest category
    /// </summary>
    public string? BigCategory { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}

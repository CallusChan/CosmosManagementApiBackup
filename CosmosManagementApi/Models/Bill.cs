using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class Bill
{
    /// <summary>
    /// Primary key for bills
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Bill id in company
    /// </summary>
    public string? BillId { get; set; }

    /// <summary>
    /// Original price
    /// </summary>
    public string? OriPrice { get; set; }

    /// <summary>
    /// Discount for this bill
    /// </summary>
    public string? Discount { get; set; }

    /// <summary>
    /// Final price of bill
    /// </summary>
    public string? FinalPrice { get; set; }

    /// <summary>
    /// When made
    /// </summary>
    public DateTime? Time { get; set; }

    /// <summary>
    /// Comments
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// PaymentMehtods
    /// </summary>
    public string? PaymentMethod { get; set; }

    public virtual ICollection<CardBill> CardBills { get; } = new List<CardBill>();

    public virtual ICollection<CustomerProductBill> CustomerProductBills { get; } = new List<CustomerProductBill>();

    public virtual ICollection<CustomerProjectBill> CustomerProjectBills { get; } = new List<CustomerProjectBill>();

    public virtual ICollection<ProductStorageBill> ProductStorageBills { get; } = new List<ProductStorageBill>();

    public virtual ICollection<StaffProductBill> StaffProductBills { get; } = new List<StaffProductBill>();

    public virtual ICollection<StaffProjectBill> StaffProjectBills { get; } = new List<StaffProjectBill>();
}

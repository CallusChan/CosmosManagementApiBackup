using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class CardBill
{
    /// <summary>
    /// Primary Key for Card Bills
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ReferenceKey to Card
    /// </summary>
    public int? CardId { get; set; }

    /// <summary>
    /// Reference key to bill
    /// </summary>
    public int? BillId { get; set; }

    /// <summary>
    /// How paid
    /// </summary>
    public string? PaymentMethod { get; set; }

    public int? StaffId { get; set; }

    public virtual Bill? Bill { get; set; }

    public virtual Card? Card { get; set; }
}

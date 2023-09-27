using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class Customer
{
    /// <summary>
    /// Primary key for the Customer
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Customer number in systen
    /// </summary>
    public string? CustomerId { get; set; }

    /// <summary>
    /// Customer chinese name
    /// </summary>
    public string? NameCn { get; set; }

    /// <summary>
    /// Customer Rating
    /// </summary>
    public string? Rating { get; set; }

    /// <summary>
    /// Customer gender
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Customer birthday
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Cusomer Phone number
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Customer Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Customer living location
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// When customer registered in system
    /// </summary>
    public DateTime? RegDate { get; set; }

    /// <summary>
    /// Customer Level
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// Customer points
    /// </summary>
    public int? Point { get; set; }

    /// <summary>
    /// path to customer img
    /// </summary>
    public string? Img { get; set; }

    /// <summary>
    /// 0 for not 1 for Deleted
    /// </summary>
    public int? IsDeleted { get; set; }

    /// <summary>
    /// Special comment of the user
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// How Knows This Company
    /// </summary>
    public string? KnoingMethod { get; set; }

    /// <summary>
    /// User Age in brief
    /// </summary>
    public string? Age { get; set; }

    /// <summary>
    /// What kind of User is it
    /// </summary>
    public string? Kind { get; set; }

    public virtual ICollection<Card> Cards { get; } = new List<Card>();

    public virtual ICollection<CustomerProductBill> CustomerProductBills { get; } = new List<CustomerProductBill>();

    public virtual ICollection<CustomerProjectBill> CustomerProjectBills { get; } = new List<CustomerProjectBill>();

    public virtual ICollection<CustomerProject> CustomerProjects { get; } = new List<CustomerProject>();
}

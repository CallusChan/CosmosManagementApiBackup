using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class CustomerProject
{
    /// <summary>
    /// Primary key for CustoemrProjects
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Reference key to customer
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Reference key to project
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// project number left
    /// </summary>
    public int? Number { get; set; }

    public virtual ICollection<Cprecord> Cprecords { get; } = new List<Cprecord>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}

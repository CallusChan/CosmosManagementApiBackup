using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class Postion
{
    /// <summary>
    /// Primary key for Position
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Postion name
    /// </summary>
    public string? PositionName { get; set; }

    /// <summary>
    /// Position number in company
    /// </summary>
    public string? PositionNo { get; set; }

    public virtual ICollection<Staff> Staff { get; } = new List<Staff>();
}

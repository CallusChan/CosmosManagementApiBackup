using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class Cprecord
{
    /// <summary>
    /// Primary key CPrecords
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// When actions happend
    /// </summary>
    public DateTime? Time { get; set; }

    /// <summary>
    /// Behaviour for Action
    /// </summary>
    public int? Behaviour { get; set; }

    /// <summary>
    /// Reference key to Customer Projects
    /// </summary>
    public int? CpId { get; set; }

    /// <summary>
    /// Path to picture
    /// </summary>
    public string? PicSrc { get; set; }

    public virtual CustomerProject? Cp { get; set; }
}

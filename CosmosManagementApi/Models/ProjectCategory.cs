using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class ProjectCategory
{
    public int Id { get; set; }

    public string? Category { get; set; }

    /// <summary>
    /// When Input TIme
    /// </summary>
    public DateTime? WritingTime { get; set; }

    /// <summary>
    /// 0 is not deleted
    /// </summary>
    public int? IsDeleted { get; set; }
}

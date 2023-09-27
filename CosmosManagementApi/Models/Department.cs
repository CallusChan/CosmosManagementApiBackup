using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class Department
{
    /// <summary>
    /// Primary Key For Department
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Department Name
    /// </summary>
    public string? DepartmentName { get; set; }

    /// <summary>
    /// Department English Name
    /// </summary>
    public string? DepartmentNameEn { get; set; }

    /// <summary>
    /// Department Registered Nmuber
    /// </summary>
    public string? RegisteredNo { get; set; }

    /// <summary>
    /// Department Number in Company
    /// </summary>
    public string? DepartmentNo { get; set; }

    public virtual ICollection<Project> Projects { get; } = new List<Project>();

    public virtual ICollection<Staff> Staff { get; } = new List<Staff>();
}

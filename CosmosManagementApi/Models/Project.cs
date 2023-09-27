using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class Project
{
    /// <summary>
    /// Primary key for project
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Project name
    /// </summary>
    public string? ProjectName { get; set; }

    /// <summary>
    /// Project period
    /// </summary>
    public string? Period { get; set; }

    /// <summary>
    /// How many times the project will do in furture
    /// </summary>
    public int? ProjectTimes { get; set; }

    /// <summary>
    /// Project introduction
    /// </summary>
    public string? Introduction { get; set; }

    /// <summary>
    /// Price for the project
    /// </summary>
    public string? ProjectPrice { get; set; }

    /// <summary>
    /// Category of the project
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Reference key to department
    /// </summary>
    public int? Department { get; set; }

    /// <summary>
    /// 0 for not deleted
    /// </summary>
    public int? IsDeleted { get; set; }

    /// <summary>
    /// Big category for Projects
    /// </summary>
    public string? BigCategory { get; set; }

    public virtual ICollection<CustomerProjectBill> CustomerProjectBills { get; } = new List<CustomerProjectBill>();

    public virtual ICollection<CustomerProject> CustomerProjects { get; } = new List<CustomerProject>();

    public virtual Department? DepartmentNavigation { get; set; }

    public virtual ICollection<StaffProjectBill> StaffProjectBills { get; } = new List<StaffProjectBill>();
}

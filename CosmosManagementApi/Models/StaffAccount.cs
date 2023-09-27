using System;
using System.Collections.Generic;

namespace CosmosManagementApi.Models;

public partial class StaffAccount
{
    /// <summary>
    /// Account primary id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Account Id
    /// </summary>
    public string? AccountId { get; set; }

    /// <summary>
    /// Account Name
    /// </summary>
    public string? AccountName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string? Pwd { get; set; }

    /// <summary>
    /// hashed password
    /// </summary>
    public string? PwdHash { get; set; }

    /// <summary>
    /// Registry TIME
    /// </summary>
    public DateTime? RegTime { get; set; }

    /// <summary>
    /// Last login time
    /// </summary>
    public DateTime? LastLogInTime { get; set; }

    /// <summary>
    /// Last login Ip
    /// </summary>
    public string? LsatLogInIp { get; set; }

    /// <summary>
    /// 1 for locked
    /// </summary>
    public int? IsLock { get; set; }

    /// <summary>
    /// 1 for Active
    /// </summary>
    public int? IsActive { get; set; }

    public int? LogInFailedTimes { get; set; }

    public DateTime? LogInFailedTime { get; set; }

    /// <summary>
    /// 5 for max level
    /// </summary>
    public int? Level { get; set; }

    /// <summary>
    /// ReferenceKey to staff
    /// </summary>
    public int? StaffId { get; set; }

    public virtual Staff? Staff { get; set; }
}

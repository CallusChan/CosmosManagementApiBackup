using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class StaffAccountCreateDto
  {

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
    /// 5 for max level
    /// </summary>
    public int? Level { get; set; }

    /// <summary>
    /// ReferenceKey to staff
    /// </summary>
    public int? StaffId { get; set; }
  }
}

using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class StaffUpdateDto
  {
    /// <summary>
    /// Staff id for staff
    /// </summary>
    public string? StaffId { get; set; }

    /// <summary>
    /// StaffName
    /// </summary>
    public string? StaffName { get; set; }

    /// <summary>
    /// Staff English name
    /// </summary>
    public string? StaffNameEn { get; set; }

    /// <summary>
    /// Staff id card number
    /// </summary>
    public string? IdCard { get; set; }

    /// <summary>
    /// Gender
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Phone number of staff
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Where the staff lives
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Staff birthday
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// When staff onboard
    /// </summary>
    public DateTime? OnboardDate { get; set; }

    /// <summary>
    /// if staff is still on board
    /// </summary>
    public int? IfOnboard { get; set; }

    /// <summary>
    /// reference key to departments
    /// </summary>
    public int? DepartmentId { get; set; }

    /// <summary>
    /// reference key to positions
    /// </summary>
    public int? PositionId { get; set; }

    public string? AccountName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Pwd { get; set; } = string.Empty;

    /// <summary>
    /// 5 for max level
    /// </summary>
    public int? Level { get; set; }

  }
}

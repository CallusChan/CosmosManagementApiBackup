using CosmosManagementApi.Models;

namespace CosmosManagementApi.Dtos
{
  public class ProjectUpdateDto
  {
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
    public string? BigCategory { get; set; }

    /// <summary>
    /// Reference key to department
    /// </summary>
    public int? Department { get; set; }

  }
}

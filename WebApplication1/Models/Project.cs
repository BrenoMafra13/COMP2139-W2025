using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Project
{
    /// <summary>
    ///  This is the primary key for the projects
    /// </summary>
    [Key]
    public int ProjectId { get; set; }
    
    /// <summary>
    /// The name of the project
    /// [Required]: Ensures this property must be set, it must have a project name
    /// </summary>
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime startDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime endDate { get; set; }
    
    public string? Status { get; set; }
    
    // One-to-Many relationship
    // This will allow EF Core to understand that one Project has potentially many ProjectTasks
    public List<ProjectTask>? Tasks { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.ProjectManagement.Models;

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
    [Display(Name = "Project Name")]
    [StringLength(100, ErrorMessage = "Project Name cannot be longer than 100 characters.")]
    public required string Name { get; set; }
    
    [Display(Name = "Project Description")]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "Project Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "Project Start Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime startDate { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "Project End Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime endDate { get; set; }
    
    [Display(Name = "Project Status")]
    [StringLength(20, ErrorMessage = "Project Status cannot be longer than 20 characters.")]
    public string? Status { get; set; }
    
    // One-to-Many relationship
    // This will allow EF Core to understand that one Project has potentially many ProjectTasks
    public List<ProjectTask>? Tasks { get; set; }
}
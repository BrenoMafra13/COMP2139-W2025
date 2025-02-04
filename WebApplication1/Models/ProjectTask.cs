using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required string Description { get; set; }
    
    // Foreign key from Project
    public int ProjectId { get; set; }
    
    // Navigation property
    // This property for easy access to the related Project entity from a ProjectTask entity
    public Project? Project { get; set; }
}
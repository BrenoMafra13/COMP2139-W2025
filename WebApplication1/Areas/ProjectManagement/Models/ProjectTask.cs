using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.ProjectManagement.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Required(ErrorMessage = "Project Title is Required")]
    [Display(Name = "Project Title")]
    [StringLength(100, ErrorMessage = "Project Title cannot be longer than 100 characters.")]
    public required string Title { get; set; }
    
    [Required(ErrorMessage = "Project Descriptio is Required")]
    [Display(Name = "Project Description")]
    [StringLength(500, ErrorMessage = "Project Description cannot be longer than 500 characters.")]
    public required string Description { get; set; }
    
    // Foreign key from Project
    [Display(Name = "Parent Project ID")]
    public int ProjectId { get; set; }
    
    // Navigation property
    // This property for easy access to the related Project entity from a ProjectTask entity
    [Display(Name = "Parent Project")]
    public Project? Project { get; set; }
}
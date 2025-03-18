using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.ProjectManagement.Models;

public class ProjectComment
{
    public int ProjectCommentId { get ; set; }
    
    [Display(Name = "Project Comment")]
    [Required]
    [StringLength(500, ErrorMessage = "Project name cannot be longer than 500 characters")]
    public string Content { get; set; }

    [DataType(DataType.DateTime)] 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    private DateTime _datePosted;
    public DateTime DatePosted
    {
        get => _datePosted; 
        set => _datePosted = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    
    // Foreign key for project
    public int ProjectId { get; set; }
    
    public Project? Project { get; set; }
}
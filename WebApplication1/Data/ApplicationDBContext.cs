using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.ProjectManagement.Models;

namespace WebApplication1.Data;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<ProjectTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Seeds Project tabel with two projects upon bootup
        modelBuilder.Entity<Project>().HasData(
            new Project {ProjectId = 1, Name = "Assignment 1", Description = "COMP2139 Assignment 1"},
            new Project {ProjectId = 2, Name = "Assignment 2", Description = "COMP2139 Assignment 2"}
        );
    }
    
}
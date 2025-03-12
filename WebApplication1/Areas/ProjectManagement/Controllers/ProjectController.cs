using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Areas.ProjectManagement.Models;

namespace WebApplication1.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("Project")]

public class ProjectController : Controller
{

    private readonly ApplicationDBContext _context;

    public ProjectController(ApplicationDBContext context)
    {
        _context = context;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var projects = await _context.Projects.ToListAsync();
        return View(projects);
    }
    
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {
        if (ModelState.IsValid)
        {
            project.startDate = project.startDate.ToUniversalTime();
            project.endDate = project.endDate.ToUniversalTime();
            
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(project);
    }
    
    // CRUD: Create - Read - Update - Delete
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        // Retrieve project from database
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound(); // Returns a 404 error if the project is not found.
        }
        return View(project);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }
    
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
    {
        if (id != project.ProjectId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Throws error if the exception cannot be identified (unknown)
                }
            }

            return RedirectToAction("Index");
        }
        return View(project);
    }
    /// <summary>
    /// Check if project Exists in DataBase
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<bool> ProjectExists(int id)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == id);
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    [HttpPost("Delete/{projectId}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return NotFound();
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search(string searchString)
    {
        var projectQuery = _context.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            searchString = searchString.ToLower();
            projectQuery = projectQuery.Where(p => p.Name.ToLower().Contains(searchString)
                                                   || (p.Description != null && p.Description.ToLower().Contains(searchString)));
        }

        var projects = await projectQuery.ToListAsync();
        return View("Index", projects);
    }


}
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
    public IActionResult Index()
    {
        var projects = _context.Projects.ToList();
        return View(projects);
    }
    
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Project project)
    {
        if (ModelState.IsValid)
        {
            project.startDate = project.startDate.ToUniversalTime();
            project.endDate = project.endDate.ToUniversalTime();
            
            _context.Projects.Add(project);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(project);
    }
    
    // CRUD: Create - Read - Update - Delete
    [HttpGet("Details/{id}")]
    public IActionResult Details(int id)
    {
        // Retrieve project from database
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound(); // Returns a 404 error if the project is not found.
        }
        return View(project);
    }

    [HttpGet("Edit/{id}")]
    public IActionResult Edit(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }
    
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
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
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.ProjectId))
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
    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }

    [HttpGet("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    [HttpPost("Delete/{projectId}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project != null)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
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
        return View("Index", projects);  // Confirme que 'Index' é a view correta
    }


}
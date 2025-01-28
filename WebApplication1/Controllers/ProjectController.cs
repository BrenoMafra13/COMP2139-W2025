using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class ProjectController : Controller
{

    private readonly ApplicationDBContext _context;

    public ProjectController(ApplicationDBContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var projects = _context.Projects.ToList();
        return View(projects);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Project project)
    {
        if (ModelState.IsValid)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(project);
    }
    
    // CRUD: Create - Read - Update - Delete
    [HttpGet]
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

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }
    
    [HttpPost]
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

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }
}
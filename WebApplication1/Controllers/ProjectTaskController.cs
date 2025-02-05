using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class ProjectTaskController : Controller
{
    private readonly ApplicationDBContext _context;

    public ProjectTaskController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(int projectId)
    {
        var tasks = _context.Tasks.Where(t => t.ProjectId == projectId).ToList();

        ViewBag.ProjectId = projectId;
        return View(tasks);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var task = _context.Tasks
            .Include(p => p.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    [HttpGet]
    public IActionResult Create(int projectId)
    {
        var project = _context.Projects.Find(projectId);

        if (project == null)
        {
            return NotFound();
        }
        
        // Create empty project for View
        var task = new ProjectTask
        {
            ProjectId = projectId,
            Title = "",
            Description = "",
        };
        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            // Return to the index action with the projectId of the task
            return RedirectToAction("Index", new {projectId = task.ProjectId});
        }

        return View(task);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var task = _context.Tasks.Include(p => p.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }

        return View(task);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var task = _context.Tasks.Include(p => p.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int ProjectTaskId)
    {
        var task = _context.Tasks.Find(ProjectTaskId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }

        return NotFound();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Areas.ProjectManagement.Models;

namespace WebApplication1.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("ProjectTask")]

public class ProjectTaskController : Controller
{
    private readonly ApplicationDBContext _context;

    public ProjectTaskController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index(int projectId)
    {
        var tasks = _context.Tasks.Where(t => t.ProjectId == projectId).ToList();
        ViewBag.ProjectId = projectId;
        return View(tasks);
    }

    [HttpGet("Details/{id}")]
    public IActionResult Details(int id)
    {
        var task = _context.Tasks.Include(t => t.Project).FirstOrDefault(t => t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpGet("Create/{projectId}")]
    public IActionResult Create(int? projectId)
    {
        if (!projectId.HasValue)
        {
            return NotFound("Project ID is required");
        }

        var project = _context.Projects.Find(projectId.Value);
        if (project == null)
        {
            return NotFound("Project not found");
        }

        var task = new ProjectTask
        {
            ProjectId = projectId.Value,
            Title = "",
            Description = ""
        };
        return View(task);
    }


    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title, Description, ProjectId")] ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }
        return View(task);
    }

    [HttpGet("Edit/{id}")]
    public IActionResult Edit(int id)
    {
        var task = _context.Tasks.Include(t => t.Project).FirstOrDefault(t => t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectTaskId, Title, Description, ProjectId")] ProjectTask task)
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

    [HttpGet("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var task = _context.Tasks.Include(t => t.Project).FirstOrDefault(t => t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost("Delete/{id}"), ActionName("Delete")]
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

    [HttpGet("Search")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        var taskQuery = _context.Tasks.AsQueryable();

        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (projectId.HasValue)
        {
            taskQuery = taskQuery.Where(t => t.ProjectId == projectId.Value);
        }

        if (searchPerformed)
        {
            taskQuery = taskQuery.Where(t => t.Title.ToLower().Contains(searchString.ToLower()) 
                                             || (t.Description != null && t.Description.ToLower().Contains(searchString.ToLower())));
        }

        var tasks = await taskQuery.ToListAsync();

        ViewBag.ProjectId = projectId;
        ViewData["SearchPerformed"] = searchPerformed;
        ViewData["searchString"] = searchString;

        return View("Index", tasks);
    }
}

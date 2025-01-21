using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class ProjectController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var projects = new List<Project>()
        {
            new Project { ProjectId = 1, Name = "Project 1", Description = "First Project"}
        };
        return View(projects);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(Project project)
    {
        // Persist new project to the DataBase
        return RedirectToAction("Index");
    }
    
    // CRUD: Create - Read - Update - Delete

    [HttpGet]
    public IActionResult Details(int id)
    {
        // Retrieve project from database
        var project = new Project { ProjectId = id, Name = "Project 1", Description = "First Project" };
        return View(project);
    }
}
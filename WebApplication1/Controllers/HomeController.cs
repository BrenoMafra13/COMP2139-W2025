using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Areas.ProjectManagement.Controllers;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        
        _logger.LogInformation("HomeController: Index action called at {Time}", DateTime.Now);
        
        return View();
    }

    public IActionResult About()
    {
        
        _logger.LogInformation("HomeController: About action called at {Time}", DateTime.Now);
        
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        searchType = searchType?.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(searchType) || string.IsNullOrWhiteSpace(searchString))
        {
            return RedirectToAction("Index", "Home");
        }

        if (searchType == "project")
        {
            return RedirectToAction(nameof(ProjectController.Search), "Project", new {searchString});
        }
        else if (searchType == "tasks")
        {
            return RedirectToAction(nameof(ProjectTaskController.Search), "ProjectTask", new { searchString });
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult NotFound(int statusCode)
    {
        if (statusCode == 404)
        {
            return View("NotFound");
        }

        return View("Error");
    }
}
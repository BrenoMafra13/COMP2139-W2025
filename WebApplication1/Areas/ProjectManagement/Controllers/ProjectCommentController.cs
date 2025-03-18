using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Areas.ProjectManagement.Models;
using WebApplication1.Data;

namespace WebApplication1.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectCommentController : Controller
{
    private readonly ApplicationDBContext _context;

    public ProjectCommentController(ApplicationDBContext _context)
    {
        _context = _context;
    }

    [HttpGet]
    public IActionResult GetComments(int projectId)
    {
        var comments = _context.ProjectComments
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.DatePosted)
            .ToList();

        return Json(comments);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
    {
        if (ModelState.IsValid)
        {
            comment.DatePosted = DateTime.Now;

            _context.ProjectComments.Add(comment);

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Comment added succesfully" });
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);

        return Json(new { success = false, message = "Invalid comment", errors = errors });

    }
}
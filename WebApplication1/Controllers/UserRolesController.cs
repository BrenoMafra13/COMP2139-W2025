using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.ProjectManagement.Models;
using WebApplication1.Models;

[Authorize(Roles = "SuperAdmin, Manager")]
public class UserRolesController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var userRolesViewModel = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            // Retrieve roles for each user asynchronously one at a time to avoid concurrency issues
            var roles = await _userManager.GetRolesAsync(user);
            userRolesViewModel.Add(new UserRolesViewModel
            {
                UserId = user.Id,  // Assegurando que UserId é passado corretamente.
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        return View(userRolesViewModel);
    }
    
    [HttpGet("UserRoles/Manage/{userId}")]
    public async Task<IActionResult> Manage(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with ID = {userId} cannot be found";
            return View("NotFound");
        }

        var roles = await _roleManager.Roles.ToListAsync();
        var userRoles = await _userManager.GetRolesAsync(user);

        var model = new ManageUserRolesViewModel
        {
            UserId = userId,
            UserName = user.UserName,
            Roles = roles.Select(role => new RoleSelection
            {
                RoleName = role.Name,
                IsSelected = userRoles.Contains(role.Name)  // Checking after fetching all roles to avoid async issues
            }).ToList()
        };

        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Manage(ManageUserRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with ID = {model.UserId} cannot be found";
            return View("NotFound");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, roles);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot remove user existing roles");
            return View(model);
        }

        var selectedRoles = model.Roles.Where(x => x.IsSelected).Select(y => y.RoleName).ToList();
        result = await _userManager.AddToRolesAsync(user, selectedRoles);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Cannot add selected roles to user");
            return View(model);
        }

        return RedirectToAction("Index");
    }

}

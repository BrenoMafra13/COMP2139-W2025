using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Areas.ProjectManagement.Models;

namespace WebApplication1.Components
{
    public class UserRoleViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return View(false);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return View(false);

            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Admin") || roles.Contains("SuperAdmin");

            return View(isAdmin);
        }

    }
}
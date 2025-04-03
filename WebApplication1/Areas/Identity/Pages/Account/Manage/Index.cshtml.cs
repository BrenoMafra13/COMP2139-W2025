using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Areas.ProjectManagement.Models;

namespace WebApplication1.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            
            public byte[]? ProfilePicture { get; set; }
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Input = new InputModel
            {
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = user.ProfilePicture  // Carrega a imagem de perfil
            };

            return Page();
        }


        [BindProperty]
        public IFormFile UploadedImage { get; set; }  // Adicionar propriedade para o arquivo carregado

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("Unable to load user with ID '{UserId}'", _userManager.GetUserId(User));
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            bool dataChanged = false;
            
            if (UploadedImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await UploadedImage.CopyToAsync(memoryStream);
                    if (memoryStream.Length < 2097152) // Limit size to 2MB
                    {
                        user.ProfilePicture = memoryStream.ToArray();
                        dataChanged = true;
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                        return Page();
                    }
                }
            }

            if (user.PhoneNumber != Input.PhoneNumber)
            {
                user.PhoneNumber = Input.PhoneNumber;
                dataChanged = true;
            }

            bool nameChanged = user.FirstName != Input.FirstName || user.LastName != Input.LastName;
            if (nameChanged && user.NameChangeLimit > 0)
            {
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.NameChangeLimit--;
                dataChanged = true;
                _logger.LogInformation("User '{UserId}' changed their name. {NameChangeLimit} name changes remain.", _userManager.GetUserId(User), user.NameChangeLimit);
            }
            else if (nameChanged)
            {
                _logger.LogInformation("User '{UserId}' attempted to change their name but no changes remain.", _userManager.GetUserId(User));
                StatusMessage = "You cannot change your name anymore.";
                return RedirectToPage();
            }
            
            if (dataChanged)
            {
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        _logger.LogError("Error updating user '{UserId}': {Error}", _userManager.GetUserId(User), error.Description);
                    }
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your profile has been updated. You can change your name " + user.NameChangeLimit + " more times.";
            }
            else
            {
                _logger.LogInformation("No changes detected for user '{UserId}'.", _userManager.GetUserId(User));
                StatusMessage = "No changes detected.";
            }

            return RedirectToPage();
        }
    }
}

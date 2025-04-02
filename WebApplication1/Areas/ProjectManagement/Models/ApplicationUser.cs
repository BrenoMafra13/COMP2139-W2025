using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Areas.ProjectManagement.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public int UserNameChangeLimit { get; set; }
    
    public byte[]? ProfilePicture { get; set; }

}
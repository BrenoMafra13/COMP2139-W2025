namespace WebApplication1.Models;

using System.Collections.Generic;

public class UserRolesViewModel
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}

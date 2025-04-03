namespace WebApplication1.Models;

public class ManageUserRolesViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public List<RoleSelection> Roles { get; set; }
}

public class RoleSelection
{
    public string RoleName { get; set; }
    public bool IsSelected { get; set; }
}

using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApp.Models;

public class ApplicationUser : IdentityUser
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }
}

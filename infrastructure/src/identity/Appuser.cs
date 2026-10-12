

using Microsoft.AspNetCore.Identity;

public class AppUser: IdentityUser
{
    // Add any additional properties or methods specific to your application user here
    public string FullName { get; set; }="";
}
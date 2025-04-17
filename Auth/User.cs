using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Auth;

public class User
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "User"; // Default role
    public DateTime RegistrationDate { get; set; }
    public string Avatar { get; set; } // Path to user's avatar image

    public User()
    {
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        Role = "User";
        RegistrationDate = DateTime.Now;
        Avatar = string.Empty;
    }

    public User(string username, string email, string password, string role = "User")
    {
        Username = username;
        Email = email;
        Password = password;
        Role = role;
        RegistrationDate = DateTime.Now;
        Avatar = string.Empty;
    }
    
}

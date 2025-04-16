using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Auth;

public class User
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }

    public User()
    {
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        RegistrationDate = DateTime.Now;
    }

    public User(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
        RegistrationDate = DateTime.Now;
    }
}

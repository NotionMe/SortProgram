using System;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public interface IAuthService
    {
        User GetCurrentUser();
        void UpdateCurrentUser(User user);
        void SetCurrentUser(User user);
        void Logout();
        bool RegisterUser(string username, string email, string password, string role = "User");
        bool LoginUser(string usernameOrEmail, string password);
        bool IsUserInRole(string role);
        bool IsAdmin();
    }
}
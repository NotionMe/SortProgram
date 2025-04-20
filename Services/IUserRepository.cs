using System.Collections.Generic;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public interface IUserRepository
    {
        bool UserExists(string username, string email);
        void SaveUser(User user);
        User? GetUserByUsernameOrEmail(string usernameOrEmail);
        User? GetUserByCredentials(string usernameOrEmail, string password);
        List<User> GetAllUsers();
        bool UpdateUser(User updatedUser);
        void UpdatePasswordsToHashed();
        List<User> GetUsersByRole(string role);
        bool UpdateUserRole(string username, string newRole);
        List<string> GetAvailableRoles();
    }
}
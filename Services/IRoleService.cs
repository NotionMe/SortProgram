// filepath: /home/notionme/Document/Repos/SortProgram/Services/IRoleService.cs
using System.Collections.Generic;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public interface IRoleService
    {
        List<User> GetUsersByRole(string role);
        bool ChangeUserRole(string username, string newRole);
        List<string> GetAvailableRoles();
    }
}
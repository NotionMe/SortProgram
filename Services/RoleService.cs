using System.Collections.Generic;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public RoleService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public List<User> GetUsersByRole(string role)
        {
            return _userRepository.GetUsersByRole(role);
        }

        public bool ChangeUserRole(string username, string newRole)
        {
            // Перевіряємо права доступу - тільки адміністратори можуть змінювати ролі
            if (!_authService.IsAdmin())
            {
                return false;
            }

            return _userRepository.UpdateUserRole(username, newRole);
        }

        public List<string> GetAvailableRoles()
        {
            return _userRepository.GetAvailableRoles();
        }
    }
}
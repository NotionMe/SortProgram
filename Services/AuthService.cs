using System;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public class AuthService : IAuthService
    {
        private User _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly IAvatarService _avatarService;

        public AuthService(IUserRepository userRepository, IAvatarService avatarService)
        {
            _currentUser = new User();
            _userRepository = userRepository;
            _avatarService = avatarService;
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public void UpdateCurrentUser(User user)
        {
            _currentUser = user;
            _userRepository.UpdateUser(user);
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }
        
        public void Logout()
        {
            // Очищаем данные текущего пользователя
            _currentUser = new User();
        }

        public bool RegisterUser(string username, string email, string password, string role = "User")
        {
            // Check if user already exists
            if (_userRepository.UserExists(username, email))
            {
                return false;
            }

            // Хешуємо пароль перед збереженням
            string hashedPassword = PasswordHasher.HashPassword(password);

            // Create new user with hashed password
            User newUser = new User(username, email, hashedPassword, role);
            
            // Assign random avatar to the user
            newUser.Avatar = _avatarService.GetRandomAvatarPath();
            
            // Save the user
            _userRepository.SaveUser(newUser);
            
            // Set as current user
            SetCurrentUser(newUser);
            
            return true;
        }

        public bool LoginUser(string usernameOrEmail, string password)
        {
            User? user = _userRepository.GetUserByUsernameOrEmail(usernameOrEmail);
            
            if (user == null)
            {
                return false;
            }
            
            // Перевіряємо хешований пароль
            if (!PasswordHasher.VerifyPassword(password, user.Password))
            {
                return false;
            }
            
            // If user doesn't have an avatar (for backward compatibility), assign one
            if (string.IsNullOrEmpty(user.Avatar))
            {
                user.Avatar = _avatarService.GetRandomAvatarPath();
                _userRepository.UpdateUser(user);
            }
            
            SetCurrentUser(user);
            return true;
        }

        // Role-related methods
        public bool IsUserInRole(string role)
        {
            return _currentUser != null && _currentUser.Role == role;
        }

        public bool IsAdmin()
        {
            return IsUserInRole("Admin");
        }
    }
}

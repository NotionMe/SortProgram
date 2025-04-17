namespace Practika2_OPAM_Ubohyi_Stanislav.Services;

using System;
using System.Collections.Generic;
using System.IO;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

public class AuthService
{
    private static AuthService? _instance;
    private User _currentUser;
    private readonly UserRepository _userRepository;
    private readonly Random _random = new Random();

    public static AuthService Instance
    {
        get
        {
            _instance ??= new AuthService();
            return _instance;
        }
    }

    private AuthService()
    {
        _currentUser = new User();
        _userRepository = new UserRepository();
    }

    public User GetCurrentUser()
    {
        return _currentUser;
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
        newUser.Avatar = GetRandomAvatarPath();
        
        // Save the user
        _userRepository.SaveUser(newUser);
        
        // Set as current user
        SetCurrentUser(newUser);
        
        return true;
    }
    
    private string GetRandomAvatarPath()
    {
        
        int avatarNumber = _random.Next(1, 5); 
        return $"avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Images/Avatar/Avatar{avatarNumber}.png";
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
            user.Avatar = GetRandomAvatarPath();
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
    public List<User> GetUsersByRole(string role)
    {
        return _userRepository.GetUsersByRole(role);
    }

    public bool ChangeUserRole(string username, string newRole)
    {
        // Only admins can change roles
        if (!IsAdmin())
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

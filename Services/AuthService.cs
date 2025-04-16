namespace Practika2_OPAM_Ubohyi_Stanislav.Services;

using System.Collections.Generic;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

public class AuthService
{
    private static AuthService? _instance;
    private User _currentUser;
    private readonly UserRepository _userRepository;

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

    public bool RegisterUser(string username, string email, string password)
    {
        // Check if user already exists
        if (_userRepository.UserExists(username, email))
        {
            return false;
        }

        // Хешуємо пароль перед збереженням
        string hashedPassword = PasswordHasher.HashPassword(password);
        
        // Create and save new user with hashed password
        var newUser = new User(username, email, hashedPassword);
        _userRepository.SaveUser(newUser);
        
        // Set as current user
        SetCurrentUser(newUser);
        
        return true;
    }

    public bool LoginUser(string usernameOrEmail, string password)
    {
        var user = _userRepository.GetUserByUsernameOrEmail(usernameOrEmail);
        
        if (user == null)
        {
            return false;
        }
        
        // Перевіряємо хешований пароль
        if (!PasswordHasher.VerifyPassword(password, user.Password))
        {
            return false;
        }
        
        SetCurrentUser(user);
        return true;
    }
}

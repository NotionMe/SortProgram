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

        // Create and save new user
        var newUser = new User(username, email, password);
        _userRepository.SaveUser(newUser);
        
        // Set as current user
        SetCurrentUser(newUser);
        
        return true;
    }

    public bool LoginUser(string usernameOrEmail, string password)
    {
        var user = _userRepository.GetUserByCredentials(usernameOrEmail, password);
        
        if (user == null)
        {
            return false;
        }
        
        SetCurrentUser(user);
        return true;
    }
}

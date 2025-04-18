using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public class UserRepository
    {
        private readonly string _filePath = "Auth/users.json";

        public bool UserExists(string username, string email)
        {
            List<User> users = GetAllUsers();
            return users.Any(u => u.Username == username || u.Email == email);
        }

        public void SaveUser(User user)
        {
            List<User> users = GetAllUsers();
            users.Add(user);
            SaveAllUsers(users);
        }

        // Новий метод для отримання користувача тільки за логіном або email
        public User? GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            List<User> users = GetAllUsers();
            return users.FirstOrDefault(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        // Старий метод збережений для сумісності
        public User? GetUserByCredentials(string usernameOrEmail, string password)
        {
            List<User> users = GetAllUsers();
            return users.FirstOrDefault(u => 
                (u.Username == usernameOrEmail || u.Email == usernameOrEmail) && 
                u.Password == password);
        }

        public List<User> GetAllUsers()
        {
            if (!File.Exists(_filePath))
            {
                return new List<User>();
            }

            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch (Exception)
            {
                return new List<User>();
            }
        }

        private void SaveAllUsers(List<User> users)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(_filePath, json);
        }
        
        // Метод для оновлення користувача
        public bool UpdateUser(User updatedUser)
        {
            var users = GetAllUsers();
            var existingUser = users.FirstOrDefault(u => u.Username == updatedUser.Username || u.Email == updatedUser.Email);
            if (existingUser != null)
            {
                // Update the user's properties
                existingUser.Username = updatedUser.Username;
                existingUser.Email = updatedUser.Email;
                existingUser.Avatar = updatedUser.Avatar;
                existingUser.Role = updatedUser.Role;
                existingUser.RegistrationDate = updatedUser.RegistrationDate;
                existingUser.Password = updatedUser.Password; // Preserve the password

                // Save changes
                SaveAllUsers(users);
                return true;
            }
            return false;
        }
        
        // Метод для оновлення паролів існуючих користувачів на хешовані
        public void UpdatePasswordsToHashed()
        {
            List<User> users = GetAllUsers();
            bool changed = false;
            
            foreach (User user in users)
            {
                // Перевіряємо, чи пароль вже хешований
                // Хеші BCrypt завжди починаються з $2a$, $2b$ або $2y$
                if (!user.Password.StartsWith("$2"))
                {
                    user.Password = PasswordHasher.HashPassword(user.Password);
                    changed = true;
                }
            }
            
            if (changed)
            {
                SaveAllUsers(users);
            }
        }

        public List<User> GetUsersByRole(string role)
        {
            List<User> users = GetAllUsers();
            return users.Where(u => u.Role == role).ToList();
        }

        public bool UpdateUserRole(string username, string newRole)
        {
            List<User> users = GetAllUsers();
            User? userToUpdate = users.FirstOrDefault(u => u.Username == username);
            
            if (userToUpdate == null)
                return false;
                
            userToUpdate.Role = newRole;
            SaveAllUsers(users);
            return true;
        }

        public List<string> GetAvailableRoles()
        {
            // List of predefined roles in the system
            return new List<string> { "User", "Admin"};
        }
    }
}
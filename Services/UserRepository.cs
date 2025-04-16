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
        private readonly string _filePath = "users.json";

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
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(_filePath, json);
        }
    }
}
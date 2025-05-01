using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public class AvatarService : IAvatarService
    {
        private readonly Random _random = new Random();
        private readonly string _avatarBaseDir = "avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Images/Avatar/";
        private readonly int _totalAvatars = 8; // Кількість доступних аватарів

        public string GetRandomAvatarPath()
        {
            int avatarNumber = _random.Next(1, _totalAvatars + 1); 
            return $"{_avatarBaseDir}Avatar{avatarNumber}.png";
        }

        public string GetDefaultAvatarPath()
        {
            return $"{_avatarBaseDir}Avatar1.png";
        }

        public bool IsValidAvatarPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
                
            if (!path.StartsWith(_avatarBaseDir))
                return false;
                
            string filename = path.Substring(_avatarBaseDir.Length);
            if (!filename.StartsWith("Avatar"))
                return false;
                
            // Перевірка, що аватар має правильний формат "AvatarN.png"
            if (!int.TryParse(filename.Substring(6, 1), out int number))
                return false;
                
            return number >= 1 && number <= _totalAvatars;
        }
        
        public Bitmap? LoadAvatar(string uri)
        {
            if (string.IsNullOrEmpty(uri))
                return null;

            try
            {
                // Use AssetLoader to load from resources
                using var stream = AssetLoader.Open(new Uri(uri));
                return new Bitmap(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image from {uri}: {ex.Message}");

                // Fallback to file-based loading
                try
                {
                    if (uri.StartsWith("avares://"))
                    {
                        string filePath = uri.Replace("avares://Practika2_OPAM_Ubohyi_Stanislav/", "");
                        string fullPath = Path.Combine(AppContext.BaseDirectory, filePath);

                        if (File.Exists(fullPath))
                        {
                            using var fileStream = File.OpenRead(fullPath);
                            return new Bitmap(fileStream);
                        }
                    }
                }
                catch (Exception fileEx)
                {
                    Console.WriteLine($"Error loading image from file: {fileEx.Message}");
                }
                
                // If all attempts fail, try to return the default avatar
                try
                {
                    using var defaultStream = AssetLoader.Open(new Uri(GetDefaultAvatarPath()));
                    return new Bitmap(defaultStream);
                }
                catch
                {
                    Console.WriteLine("Failed to load even the default avatar");
                }
            }
            return null;
        }
        
        public List<string> GetAllAvatarPaths()
        {
            List<string> avatarPaths = new List<string>();
            
            for (int i = 1; i <= _totalAvatars; i++)
            {
                avatarPaths.Add($"{_avatarBaseDir}Avatar{ i}.png");
            }
            
            return avatarPaths;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services
{
    public class LanguageManager : INotifyPropertyChanged
    {
        private static LanguageManager? _instance;
        private Dictionary<string, string>? _currentLanguageStrings;
        private string _currentLanguage = "en"; // Default language
        private string? _cachedLocalizationPath;

        public static LanguageManager Instance 
        { 
            get 
            {
                if (_instance == null)
                {
                    _instance = new LanguageManager();
                }
                return _instance;
            } 
        }

        // Add indexer for XAML binding
        public string this[string key]
        {
            get => GetString(key);
        }

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    LoadLanguage(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
                    OnLanguageChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler LanguageChanged = delegate { };

        private LanguageManager()
        {
            LoadLanguage(_currentLanguage);
        }

        public void LoadLanguage(string languageCode)
        {
            try
            {
                string filePath = FindLocalizationFile(languageCode);
                
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);
                    _currentLanguageStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
                    
                    // Add localization strings to Avalonia resources
                    if (_currentLanguageStrings != null)
                    {
                        Avalonia.Controls.IResourceDictionary? resources = Application.Current?.Resources;
                        if (resources != null)
                        {
                            foreach (KeyValuePair<string, string> kvp in _currentLanguageStrings)
                            {
                                resources[kvp.Key] = kvp.Value;
                            }
                        }
                    }
                }
                else
                {
                    _currentLanguageStrings = new Dictionary<string, string>();
                }
            }
            catch (Exception)
            {
                _currentLanguageStrings = new Dictionary<string, string>();
            }
        }

        private string FindLocalizationFile(string languageCode)
        {
            // Якщо шлях вже кешований, використовуємо його
            if (!string.IsNullOrEmpty(_cachedLocalizationPath))
            {
                try {
                    string directory = Path.GetDirectoryName(_cachedLocalizationPath) ?? string.Empty;
                    string filePath = Path.Combine(directory, $"{languageCode}.json");
                    if (File.Exists(filePath))
                    {
                        return filePath;
                    }
                }
                catch (Exception) { /* Ігноруємо помилки з кешованим шляхом */ }
            }
            
            // Можливі шляхи до файлів локалізації
            string currentDirectory = Directory.GetCurrentDirectory();
            List<string> baseLocalizationDirs = new List<string> {
                Path.Combine("Assets", "Localization"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Localization"),
                Path.Combine(currentDirectory, "Assets", "Localization"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Localization")
            };

            // Додаємо Windows-специфічні шляхи
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                string exeDir = AppContext.BaseDirectory;
                string? parentDir = Directory.GetParent(exeDir)?.FullName;
                if (!string.IsNullOrEmpty(parentDir))
                {
                    baseLocalizationDirs.Add(Path.Combine(parentDir, "Assets", "Localization"));
                    
                    string? grandParentDir = Directory.GetParent(parentDir)?.FullName;
                    if (!string.IsNullOrEmpty(grandParentDir))
                    {
                        baseLocalizationDirs.Add(Path.Combine(grandParentDir, "Assets", "Localization"));
                    }
                }
            }

            // Шукаємо файл у всіх можливих директоріях
            foreach (string baseDir in baseLocalizationDirs.Distinct())
            {
                if (Directory.Exists(baseDir))
                {
                    string filePath = Path.Combine(baseDir, $"{languageCode}.json");
                    if (File.Exists(filePath))
                    {
                        _cachedLocalizationPath = filePath;
                        return filePath;
                    }
                }
            }

            // Спроба створити і скопіювати файл для Windows
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && string.IsNullOrEmpty(_cachedLocalizationPath))
            {
                try
                {
                    string localizationDir = Path.Combine(AppContext.BaseDirectory, "Assets", "Localization");
                    
                    if (!Directory.Exists(localizationDir))
                    {
                        Directory.CreateDirectory(localizationDir);
                    }
                    
                    string tempFilePath = Path.Combine(localizationDir, $"{languageCode}.json");
                    
                    if (CopyEmbeddedResource(languageCode, tempFilePath))
                    {
                        _cachedLocalizationPath = tempFilePath;
                        return tempFilePath;
                    }
                }
                catch (Exception) 
                {
                    
                }
            }

            return string.Empty;
        }

        private bool CopyEmbeddedResource(string languageCode, string targetPath)
        {
            try
            {
                string resourceKey = $"avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Localization/{languageCode}.json";
                Uri uri = new Uri(resourceKey);
                
                if (Avalonia.Platform.AssetLoader.Exists(uri))
                {
                    using (Stream stream = Avalonia.Platform.AssetLoader.Open(uri))
                    using (FileStream fileStream = File.Create(targetPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                // Помилка копіювання вбудованого ресурсу
            }
            return false;
        }

        public string GetString(string key)
        {
            if (_currentLanguageStrings != null && _currentLanguageStrings.TryGetValue(key, out string? value) && !string.IsNullOrEmpty(value))
            {
                return value;
            }
            return key; // Return the key itself if the translation is not found
        }

        public void SetLanguage(string languageCode)
        {
            CurrentLanguage = languageCode;
        }

        private void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        // Додаємо публічний метод для примусового оновлення локалізацій
        public void ForceUpdate()
        {
            LoadLanguage(_currentLanguage);
            
            if (_currentLanguageStrings != null)
            {
                Avalonia.Controls.IResourceDictionary? resources = Application.Current?.Resources;
                if (resources != null)
                {
                    foreach (KeyValuePair<string, string> kvp in _currentLanguageStrings)
                    {
                        resources[kvp.Key] = kvp.Value;
                    }
                }
            }
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            OnLanguageChanged();
        }

        public List<string> GetAvailableLanguages()
        {
            List<string> defaultLanguages = new List<string> { "en", "uk" };
            
            if (string.IsNullOrEmpty(_cachedLocalizationPath))
            {
                return defaultLanguages;
            }
            
            string? directory = Path.GetDirectoryName(_cachedLocalizationPath);
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            {
                return defaultLanguages;
            }
            
            try
            {
                var files = Directory.GetFiles(directory, "*.json")
                    .Select(Path.GetFileNameWithoutExtension)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList();
                    
                return files.Count > 0 ? files! : defaultLanguages;
            }
            catch (Exception)
            {
                return defaultLanguages;
            }
        }
    }
}
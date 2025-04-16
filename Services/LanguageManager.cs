using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia;
using System.Diagnostics;

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
                Debug.WriteLine($"Attempting to load language file from: {filePath}");
                
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);
                    _currentLanguageStrings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
                    Debug.WriteLine($"Successfully loaded language file with {_currentLanguageStrings?.Count ?? 0} entries");
                    
                    // Add localization strings to Avalonia resources
                    if (_currentLanguageStrings != null)
                    {
                        var resources = Application.Current?.Resources;
                        if (resources != null)
                        {
                            foreach (var kvp in _currentLanguageStrings)
                            {
                                resources[kvp.Key] = kvp.Value;
                            }
                        }
                    }
                }
                else
                {
                    Debug.WriteLine($"Language file not found at path: {filePath}");
                    _currentLanguageStrings = new Dictionary<string, string>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading language file: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                _currentLanguageStrings = new Dictionary<string, string>();
            }
        }

        private string FindLocalizationFile(string languageCode)
        {
            // Спочатку виводимо поточний робочий каталог для діагностики
            string currentDirectory = Directory.GetCurrentDirectory();
            Debug.WriteLine($"Current directory: {currentDirectory}");
            
            // Якщо шлях вже кешований, використовуємо його
            if (!string.IsNullOrEmpty(_cachedLocalizationPath))
            {
                try {
                    string directory = Path.GetDirectoryName(_cachedLocalizationPath) ?? string.Empty;
                    string filePath = Path.Combine(directory, $"{languageCode}.json");
                    Debug.WriteLine($"Checking cached path: {filePath}");
                    if (File.Exists(filePath))
                        return filePath;
                }
                catch (Exception ex) {
                    Debug.WriteLine($"Error with cached path: {ex.Message}");
                }
            }
            
            // Можливі шляхи до файлів локалізації у порядку пріоритету
            List<string> possiblePaths = new List<string>
            {
                Path.Combine("Assets", "Localization", $"{languageCode}.json"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Localization", $"{languageCode}.json"),
                // Додаткові шляхи для Windows
                Path.Combine(currentDirectory, "Assets", "Localization", $"{languageCode}.json"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Localization", $"{languageCode}.json")
            };

            // Додаємо шляхи відносно виконуваного файлу
            string? executablePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location);
            if (!string.IsNullOrEmpty(executablePath))
            {
                possiblePaths.Add(Path.Combine(executablePath, "Assets", "Localization", $"{languageCode}.json"));
            }

            foreach (string path in possiblePaths)
            {
                Debug.WriteLine($"Checking path: {path}");
                if (File.Exists(path))
                {
                    Debug.WriteLine($"Found localization file at: {path}");
                    _cachedLocalizationPath = path;
                    return path;
                }
            }

            Debug.WriteLine($"No localization file found for language: {languageCode}");
            return string.Empty;
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
            
            // Додаткова перевірка, що ресурси оновлені в Avalonia Resources
            if (_currentLanguageStrings != null)
            {
                var resources = Application.Current?.Resources;
                if (resources != null)
                {
                    foreach (var kvp in _currentLanguageStrings)
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
            // Стандартні мови, які точно існують
            List<string> languages = new List<string> { "en", "uk" };
            
            try
            {
                string? directory = Path.GetDirectoryName(_cachedLocalizationPath);
                if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
                {
                    Debug.WriteLine($"Looking for language files in directory: {directory}");
                    var langFiles = Directory.GetFiles(directory, "*.json")
                        .Select(Path.GetFileNameWithoutExtension)
                        .Where(name => name != null)
                        .Cast<string>()
                        .ToList();
                    
                    Debug.WriteLine($"Found language files: {string.Join(", ", langFiles)}");
                    return langFiles;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting available languages: {ex.Message}");
            }
            
            return languages;
        }
    }
}
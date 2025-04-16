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
            List<string> possiblePaths = new List<string>();

            // Базові шляхи, які будемо комбінувати
            List<string> baseLocalizationDirs = new List<string> {
                Path.Combine("Assets", "Localization"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Localization"),
                Path.Combine(currentDirectory, "Assets", "Localization"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Localization")
            };

            // Додаємо шляхи відносно виконуваного файлу
            try
            {
                string? executablePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location);
                if (!string.IsNullOrEmpty(executablePath))
                {
                    baseLocalizationDirs.Add(Path.Combine(executablePath, "Assets", "Localization"));
                }
                
                // Для Windows також додаємо шляхи з кореневої директорії і батьківських каталогів
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    string? exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    if (!string.IsNullOrEmpty(exePath))
                    {
                        string? exeDir = Path.GetDirectoryName(exePath);
                        if (!string.IsNullOrEmpty(exeDir))
                        {
                            baseLocalizationDirs.Add(Path.Combine(exeDir, "Assets", "Localization"));
                            
                            // Перевіряємо також каталоги відносно батьківського каталогу 
                            string? parentDir = Directory.GetParent(exeDir)?.FullName;
                            if (!string.IsNullOrEmpty(parentDir))
                            {
                                baseLocalizationDirs.Add(Path.Combine(parentDir, "Assets", "Localization"));
                                
                                // Ще один рівень вгору (для Debug/Release/net9.0)
                                string? grandParentDir = Directory.GetParent(parentDir)?.FullName;
                                if (!string.IsNullOrEmpty(grandParentDir))
                                {
                                    baseLocalizationDirs.Add(Path.Combine(grandParentDir, "Assets", "Localization"));
                                }
                            }
                        }
                    }
                    
                    // Додаємо Windows-специфічні шляхи з зворотними слешами
                    string winPath = currentDirectory + "\\Assets\\Localization";
                    baseLocalizationDirs.Add(winPath);
                    
                    string exeDirWin = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
                    baseLocalizationDirs.Add(exeDirWin + "\\Assets\\Localization");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting assembly paths: {ex.Message}");
            }

            // Перевіряємо кожен базовий шлях
            foreach (string baseDir in baseLocalizationDirs.Distinct())
            {
                // Переконуємося, що директорія існує перед перевіркою файлу
                if (Directory.Exists(baseDir))
                {
                    string filePath = Path.Combine(baseDir, $"{languageCode}.json");
                    possiblePaths.Add(filePath);
                    
                    // Логуємо знайдені директорії
                    Debug.WriteLine($"Found directory: {baseDir}");
                    try
                    {
                        string[] files = Directory.GetFiles(baseDir, "*.json");
                        Debug.WriteLine($"Available language files in {baseDir}: {string.Join(", ", files)}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error listing files in {baseDir}: {ex.Message}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Directory does not exist: {baseDir}");
                }
            }

            // Видаляємо дублікати
            possiblePaths = possiblePaths.Distinct().ToList();

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

            // Спеціальний код для Windows - спроба створити директорії та файли локалізації, якщо їх немає
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && string.IsNullOrEmpty(_cachedLocalizationPath))
            {
                try
                {
                    string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
                    string localizationDir = Path.Combine(exeDir, "Assets", "Localization");
                    
                    // Створюємо директорію, якщо її немає
                    if (!Directory.Exists(localizationDir))
                    {
                        Directory.CreateDirectory(localizationDir);
                        Debug.WriteLine($"Created directory: {localizationDir}");
                    }
                    
                    // Створюємо порожній файл локалізації, щоб знати шлях для наступного разу
                    string tempFilePath = Path.Combine(localizationDir, $"{languageCode}.json");
                    
                    // Копіюємо вбудований файл локалізації, якщо він існує в ресурсах
                    if (CopyEmbeddedResource(languageCode, tempFilePath))
                    {
                        Debug.WriteLine($"Created localization file from embedded resource: {tempFilePath}");
                        _cachedLocalizationPath = tempFilePath;
                        return tempFilePath;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error creating localization directory/file: {ex.Message}");
                }
            }

            Debug.WriteLine($"No localization file found for language: {languageCode}");
            return string.Empty;
        }

        private bool CopyEmbeddedResource(string languageCode, string targetPath)
        {
            try
            {
                // Шукаємо ресурс Avalonia з локалізацією
                var resourceKey = $"avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Localization/{languageCode}.json";
                var uri = new Uri(resourceKey);
                
                if (Avalonia.Platform.AssetLoader.Exists(uri))
                {
                    using (var stream = Avalonia.Platform.AssetLoader.Open(uri))
                    using (var fileStream = File.Create(targetPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error copying embedded resource: {ex.Message}");
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
using System;
using System.Globalization;
using System.Threading;
using System.Resources;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Avalonia.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Practika2_OPAM_Ubohyi_Stanislav.Utils;

public static class LocalizationService
{
    public static event EventHandler? LanguageChanged;
    
    public static void SetLanguage(string cultureName)
    {
        Debug.WriteLine($"Setting language to: {cultureName}");
        
        // Use the LanguageManager to load the language
        LanguageManager.Instance.LoadLanguage(cultureName);
        
        // Notify subscribers that language has changed
        LanguageChanged?.Invoke(null, EventArgs.Empty);
        
        // Force reload of the main window UI after language change
        try 
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Debug.WriteLine($"MainWindow type: {desktop.MainWindow?.GetType().FullName}");
                
                // Універсальне оновлення вікна незалежно від його типу
                var mainWindow = desktop.MainWindow;
                if (mainWindow != null)
                {
                    // Спочатку шукаємо ContentBorder
                    var contentBorder = mainWindow.FindControl<Border>("ContentBorder");
                    Debug.WriteLine($"ContentBorder found: {contentBorder != null}");
                    
                    if (contentBorder?.Child is Control existingPage)
                    {
                        // Запам'ятовуємо тип поточної сторінки
                        Type pageType = existingPage.GetType();
                        Debug.WriteLine($"Current page type: {pageType.FullName}");
                        
                        // Перевіряємо, чи це сторінка налаштувань
                        bool isSettingsPage = pageType.Name.Contains("Settings") || 
                                             pageType.FullName?.Contains(".Settings") == true;
                        
                        // Якщо це не сторінка налаштувань, оновлюємо сторінку
                        if (!isSettingsPage)
                        {
                            Debug.WriteLine("Recreating page for localization update");
                            try
                            {
                                if (Activator.CreateInstance(pageType) is Control newPage)
                                {
                                    contentBorder.Child = newPage;
                                    mainWindow.InvalidateVisual();
                                    Debug.WriteLine("Page recreated successfully");
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error recreating page: {ex.Message}");
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Not recreating settings page to avoid loops");
                        }
                    }
                    else
                    {
                        // Якщо не знайшли ContentBorder, спробуємо оновити всі дочірні елементи
                        Debug.WriteLine("ContentBorder not found, invalidating main window");
                        mainWindow.InvalidateVisual();
                    }
                }
            }
            else
            {
                Debug.WriteLine("Not a classic desktop application lifetime");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating UI after language change: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
    
    // Helper method to get the current culture name
    public static string GetCurrentLanguage()
    {
        return LanguageManager.Instance.CurrentLanguage;
    }
    
    // Метод для діагностики місця знаходження файлів локалізації
    public static void DiagnoseLocalizationFiles()
    {
        try
        {
            string currentDir = Directory.GetCurrentDirectory();
            Debug.WriteLine($"Current directory: {currentDir}");
            Debug.WriteLine($"OS Platform: {Environment.OSVersion.Platform}");
            
            // Перевіряємо базові папки
            var basePaths = new List<string>
            {
                Path.Combine("Assets", "Localization"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Localization"),
                Path.Combine(currentDir, "Assets", "Localization"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "Localization")
            };
            
            // Для Windows додаємо специфічні шляхи
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                string? executablePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (!string.IsNullOrEmpty(executablePath))
                {
                    basePaths.Add(Path.Combine(executablePath, "Assets", "Localization"));
                    
                    // Також перевіримо батьківські каталоги (для Debug/Release структури)
                    string? parentDir = Directory.GetParent(executablePath)?.FullName;
                    if (!string.IsNullOrEmpty(parentDir))
                    {
                        basePaths.Add(Path.Combine(parentDir, "Assets", "Localization"));
                        
                        // Ще один рівень вгору
                        string? grandParentDir = Directory.GetParent(parentDir)?.FullName;
                        if (!string.IsNullOrEmpty(grandParentDir))
                        {
                            basePaths.Add(Path.Combine(grandParentDir, "Assets", "Localization"));
                        }
                    }
                }
            }
            
            foreach (string path in basePaths.Distinct())
            {
                Debug.WriteLine($"Checking directory: {path}");
                if (Directory.Exists(path))
                {
                    Debug.WriteLine($"Directory exists: {path}");
                    var files = Directory.GetFiles(path, "*.json");
                    Debug.WriteLine($"Found files: {string.Join(", ", files)}");
                    
                    // Перевіряємо конкретні файли локалізації
                    if (File.Exists(Path.Combine(path, "en.json")))
                        Debug.WriteLine("en.json exists!");
                    if (File.Exists(Path.Combine(path, "uk.json")))
                        Debug.WriteLine("uk.json exists!");
                }
                else
                {
                    Debug.WriteLine($"Directory does NOT exist: {path}");
                    
                    // Спробуємо створити директорію, якщо це можливо
                    try 
                    {
                        // Перевіряємо, чи існує батьківський каталог
                        string? parentDir = Path.GetDirectoryName(path);
                        if (!string.IsNullOrEmpty(parentDir) && Directory.Exists(parentDir)) 
                        {
                            Debug.WriteLine($"Creating directory: {path}");
                            Directory.CreateDirectory(path);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Couldn't create directory: {ex.Message}");
                    }
                }
            }
            
            // Перевіряємо вміст завантаженої поточної мови
            Debug.WriteLine($"Current language from LanguageManager: {LanguageManager.Instance.CurrentLanguage}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error diagnosing localization files: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}
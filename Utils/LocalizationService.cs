using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav.Utils;

/// <summary>
/// Service responsible for managing application localization.
/// </summary>
public static class LocalizationService
{
    private const string LocalizationFolder = "Localization";
    private const string AssetsFolder = "Assets";
    
    /// <summary>
    /// Event that fires when the application language changes.
    /// </summary>
    public static event EventHandler? LanguageChanged;
    
    /// <summary>
    /// Sets the application language to the specified culture.
    /// </summary>
    /// <param name="cultureName">Culture code (e.g., "en", "uk")</param>
    public static void SetLanguage(string cultureName)
    {
        // Load the language using LanguageManager
        LanguageManager.Instance.LoadLanguage(cultureName);
        
        // Notify subscribers that language has changed
        LanguageChanged?.Invoke(null, EventArgs.Empty);
        
        // Update UI with new language
        UpdateUserInterface();
    }
    
    /// <summary>
    /// Gets the current application language.
    /// </summary>
    /// <returns>Current language code (e.g., "en", "uk")</returns>
    public static string GetCurrentLanguage()
    {
        return LanguageManager.Instance.CurrentLanguage;
    }
    
    /// <summary>
    /// Diagnoses localization files availability and location.
    /// </summary>
    public static void DiagnoseLocalizationFiles()
    {
        try
        {
            string currentDir = Directory.GetCurrentDirectory();
            List<string> localizationPaths = GetLocalizationPaths(currentDir);
            
            foreach (string path in localizationPaths.Distinct())
            {
                CheckLocalizationDirectory(path);
            }
            
            // Log current language
            Debug.WriteLine($"Current language: {LanguageManager.Instance.CurrentLanguage}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error diagnosing localization files: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Updates the user interface after a language change.
    /// </summary>
    private static void UpdateUserInterface()
    {
        try 
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Window? mainWindow = desktop.MainWindow;
                if (mainWindow != null)
                {
                    UpdateWindowContent(mainWindow);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating UI after language change: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Updates the content of a window after a language change.
    /// </summary>
    /// <param name="mainWindow">Window to update</param>
    private static void UpdateWindowContent(Window mainWindow)
    {
        // Find content border
        Border? contentBorder = mainWindow.FindControl<Border>("ContentBorder");
        
        if (contentBorder?.Child is Control existingPage)
        {
            Type pageType = existingPage.GetType();
            bool isSettingsPage = IsSettingsPage(pageType);
            
            if (!isSettingsPage)
            {
                RecreatePageContent(contentBorder, pageType);
            }
        }
        else
        {
            // If ContentBorder not found, update the entire window
            mainWindow.InvalidateVisual();
        }
    }
    
    /// <summary>
    /// Recreates a page content to apply new localization.
    /// </summary>
    /// <param name="contentBorder">Border containing the page</param>
    /// <param name="pageType">Type of page to recreate</param>
    private static void RecreatePageContent(Border contentBorder, Type pageType)
    {
        try
        {
            if (Activator.CreateInstance(pageType) is Control newPage)
            {
                contentBorder.Child = newPage;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error recreating page: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Determines if a page is a settings page.
    /// </summary>
    /// <param name="pageType">Page type to check</param>
    /// <returns>True if it's a settings page, otherwise false</returns>
    private static bool IsSettingsPage(Type pageType)
    {
        return pageType.Name.Contains("Settings") || 
               pageType.FullName?.Contains(".Settings") == true;
    }
    
    /// <summary>
    /// Gets all potential localization file paths.
    /// </summary>
    /// <param name="currentDir">Current directory</param>
    /// <returns>List of potential localization paths</returns>
    private static List<string> GetLocalizationPaths(string currentDir)
    {
        List<string> paths = new List<string>
        {
            Path.Combine(AssetsFolder, LocalizationFolder),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AssetsFolder, LocalizationFolder),
            Path.Combine(currentDir, AssetsFolder, LocalizationFolder),
            Path.Combine(AppContext.BaseDirectory, AssetsFolder, LocalizationFolder)
        };
        
        // Add Windows-specific paths if needed
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            AddWindowsSpecificPaths(paths);
        }
        
        return paths;
    }
    
    /// <summary>
    /// Adds Windows-specific localization paths.
    /// </summary>
    /// <param name="paths">List to add paths to</param>
    private static void AddWindowsSpecificPaths(List<string> paths)
    {
        string executablePath = AppContext.BaseDirectory;
        paths.Add(Path.Combine(executablePath, AssetsFolder, LocalizationFolder));
            
        string? parentDir = Directory.GetParent(executablePath)?.FullName;
        if (!string.IsNullOrEmpty(parentDir))
        {
            paths.Add(Path.Combine(parentDir, AssetsFolder, LocalizationFolder));
            
            string? grandParentDir = Directory.GetParent(parentDir)?.FullName;
            if (!string.IsNullOrEmpty(grandParentDir))
            {
                paths.Add(Path.Combine(grandParentDir, AssetsFolder, LocalizationFolder));
            }
        }
    }
    
    /// <summary>
    /// Checks a localization directory for localization files.
    /// </summary>
    /// <param name="path">Directory path to check</param>
    private static void CheckLocalizationDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path, "*.json");
            Debug.WriteLine($"Found localization files in {path}: {string.Join(", ", files)}");
        }
        else
        {
            TryCreateLocalizationDirectory(path);
        }
    }
    
    /// <summary>
    /// Attempts to create a localization directory if it doesn't exist.
    /// </summary>
    /// <param name="path">Directory path to create</param>
    private static void TryCreateLocalizationDirectory(string path)
    {
        try 
        {
            string? parentDir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(parentDir) && Directory.Exists(parentDir)) 
            {
                Directory.CreateDirectory(path);
                Debug.WriteLine($"Created localization directory: {path}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Couldn't create localization directory: {ex.Message}");
        }
    }
}
using System;
using System.Globalization;
using System.Threading;
using System.Resources;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using Avalonia.Controls;
using System.ComponentModel;

namespace Practika2_OPAM_Ubohyi_Stanislav.Utils;

public static class LocalizationService
{
    public static event EventHandler? LanguageChanged;
    
    public static void SetLanguage(string cultureName)
    {
        // Use the LanguageManager to load the language
        LanguageManager.Instance.LoadLanguage(cultureName);
        
        // Notify subscribers that language has changed
        LanguageChanged?.Invoke(null, EventArgs.Empty);
        
        // Force reload of the main window UI after language change
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow is SortProgram mainWindow)
            {
                // Save the current page
                var contentBorder = mainWindow.FindControl<Border>("ContentBorder");
                Control? currentPage = contentBorder?.Child as Control;
                
                // Check if current page is SettingsPage
                bool isSettingsPage = currentPage is Pages.SettingsPage;
                
                // Update the UI of the entire application
                if (contentBorder?.Child is Control existingPage)
                {
                    // If it's not the settings page, recreate it
                    if (!isSettingsPage)
                    {
                        Type pageType = existingPage.GetType();
                        if (Activator.CreateInstance(pageType) is Control newPage)
                        {
                            contentBorder.Child = newPage;
                            mainWindow.InvalidateVisual();
                        }
                    }
                }
            }
        }
    }
    
    // Helper method to get the current culture name
    public static string GetCurrentLanguage()
    {
        return LanguageManager.Instance.CurrentLanguage;
    }
}
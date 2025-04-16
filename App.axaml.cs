using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using System;
using System.IO;

namespace Practika2_OPAM_Ubohyi_Stanislav;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        // Диагностика локализационных файлов перед запуском
        Utils.LocalizationService.DiagnoseLocalizationFiles();
        
        // Initialize language with default setting (en)
        LanguageManager.Instance.LoadLanguage("en");
        
        // Дополнительное принудительное обновление для Windows
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            System.Diagnostics.Debug.WriteLine("Windows platform detected, forcing language update");
            LanguageManager.Instance.ForceUpdate();
            
            // Переконуємось, що необхідні директорії існують
            try
            {
                string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
                string localizationDir = Path.Combine(exeDir, "Assets", "Localization");
                
                if (!Directory.Exists(Path.Combine(exeDir, "Assets")))
                {
                    Directory.CreateDirectory(Path.Combine(exeDir, "Assets"));
                    System.Diagnostics.Debug.WriteLine($"Created Assets directory: {Path.Combine(exeDir, "Assets")}");
                }
                
                if (!Directory.Exists(localizationDir))
                {
                    Directory.CreateDirectory(localizationDir);
                    System.Diagnostics.Debug.WriteLine($"Created Localization directory: {localizationDir}");
                    
                    // Копіюємо файли локалізації з ресурсів, якщо вони існують
                    CopyLocalizationFilesToOutputDirectory(localizationDir);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating directories: {ex.Message}");
            }
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new LoginMenu();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void CopyLocalizationFilesToOutputDirectory(string localizationDir)
    {
        try
        {
            // Копіюємо файли локалізації
            string[] languages = { "en", "uk" };
            
            foreach (string lang in languages)
            {
                var resourceKey = $"avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Localization/{lang}.json";
                var uri = new Uri(resourceKey);
                
                if (Avalonia.Platform.AssetLoader.Exists(uri))
                {
                    string targetPath = Path.Combine(localizationDir, $"{lang}.json");
                    System.Diagnostics.Debug.WriteLine($"Copying localization file {lang}.json to {targetPath}");
                    
                    using (var stream = Avalonia.Platform.AssetLoader.Open(uri))
                    using (var fileStream = File.Create(targetPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Resource not found: {resourceKey}");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error copying localization files: {ex.Message}");
        }
    }
}
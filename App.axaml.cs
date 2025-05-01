using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        // Налаштування сервісів за допомогою DI
        ConfigureServices();
        
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
                string exeDir = AppContext.BaseDirectory;
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

    private void ConfigureServices()
    {
        var services = new ServiceCollection();
        
        // Реєстрація сервісів
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IAvatarService, AvatarService>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IRoleService, RoleService>();
        
        // Створення провайдера сервісів
        ServiceProvider = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new LoginMenu();
        }

        base.OnFrameworkInitializationCompleted();
    }

    // Отримання сервісу через DI
    public static T GetService<T>() where T : class
    {
        return ServiceProvider!.GetService<T>() ?? throw new InvalidOperationException($"Service {typeof(T).Name} not found");
    }

    private void CopyLocalizationFilesToOutputDirectory(string localizationDir)
    {
        try
        {
            // Копіюємо файли локалізації
            string[] languages = { "en", "uk" };
            
            foreach (string lang in languages)
            {
                string resourceKey = $"avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Localization/{lang}.json";
                Uri uri = new Uri(resourceKey);
                
                if (Avalonia.Platform.AssetLoader.Exists(uri))
                {
                    string targetPath = Path.Combine(localizationDir, $"{lang}.json");
                    System.Diagnostics.Debug.WriteLine($"Copying localization file {lang}.json to {targetPath}");
                    
                    using (Stream stream = Avalonia.Platform.AssetLoader.Open(uri))
                    using (FileStream fileStream = File.Create(targetPath))
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
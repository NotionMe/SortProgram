using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Avalonia.Diagnostics;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;
using Avalonia.Controls;

namespace Practika2_OPAM_Ubohyi_Stanislav;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        ConfigureServices();

        Utils.LocalizationService.DiagnoseLocalizationFiles();

        LanguageManager.Instance.LoadLanguage("en");

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            LanguageManager.Instance.ForceUpdate();

            try
            {
                string exeDir = AppContext.BaseDirectory;
                string localizationDir = Path.Combine(exeDir, "Assets", "Localization");

                if (!Directory.Exists(Path.Combine(exeDir, "Assets")))
                {
                    Directory.CreateDirectory(Path.Combine(exeDir, "Assets"));
                }

                if (!Directory.Exists(localizationDir))
                {
                    Directory.CreateDirectory(localizationDir);
                    CopyLocalizationFilesToOutputDirectory(localizationDir);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directories: {ex.Message}");
            }
        }
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IAvatarService, AvatarService>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IRoleService, RoleService>();

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

    public static T GetService<T>() where T : class
    {
        return ServiceProvider!.GetService<T>() ?? throw new InvalidOperationException($"Service {typeof(T).Name} not found");
    }

    private void CopyLocalizationFilesToOutputDirectory(string localizationDir)
    {
        try
        {
            string[] languages = { "en", "uk" };

            foreach (string lang in languages)
            {
                string resourceKey = $"avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Localization/{lang}.json";
                Uri uri = new Uri(resourceKey);

                if (Avalonia.Platform.AssetLoader.Exists(uri))
                {
                    string targetPath = Path.Combine(localizationDir, $"{lang}.json");

                    using (Stream stream = Avalonia.Platform.AssetLoader.Open(uri))
                    using (FileStream fileStream = File.Create(targetPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error copying localization files: {ex.Message}");
        }
    }
}
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        // Initialize language with default setting (en)
        LanguageManager.Instance.LoadLanguage("en");
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new LoginMenu();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
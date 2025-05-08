using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using Practika2_OPAM_Ubohyi_Stanislav.Pages;
using Practika2_OPAM_Ubohyi_Stanislav.Styles;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.InfoAlgoritm;

namespace Practika2_OPAM_Ubohyi_Stanislav
{
    public partial class SortProgram : Window
    {
        private Border? currentSelectedButton;
        public IAuthService AuthService { get; }
        public bool IsDarkTheme => ThemeManager.IsDarkTheme;

        public SortProgram()
        {
            InitializeComponent();
            
            // Get auth service through DI
            AuthService = App.GetService<IAuthService>(); // Assign to the public property
            
            // Initialize the language manager with default language
            // You can get system language or use saved user preference
            LanguageManager.Instance.LoadLanguage("en");

            NavigateToPage(new Algoritm());
            UpdateSelectedButton(HomeButton);
            
            SetupEventHandlers();

            Grid? mainGrid = this.FindControl<Grid>("MainGrid");
            if (mainGrid != null)
            {
                ThemeManager.SetLightTheme(mainGrid);
            }
            UpdateThemeUI();
        }
        
        private void SetupEventHandlers()
        {
            // Головні кнопки навігації
            HomeButton.PointerPressed += (s, e) => {
                UpdateSelectedButton(HomeButton);
                NavigateToPage(new HomePage());
            };
            
            SearchButton.PointerPressed += (s, e) => {
                UpdateSelectedButton(SearchButton);
                NavigateToPage(new SortingAlgorithmsPage());
            };
            
            StatisticsButton.PointerPressed += (s, e) => {
                UpdateSelectedButton(StatisticsButton);
                NavigateToPage(new StatisticsPage());
            };
            
            SettingsButton.PointerPressed += (s, e) => {
                UpdateSelectedButton(SettingsButton);
                NavigateToPage(new SettingsPage());
            };
            
            ProfileButton.PointerPressed += (s, e) => {
                UpdateSelectedButton(ProfileButton);
                NavigateToPage(new ProfilePage());
            };
            
            // Обробка натискання кнопки Admin
            AdminButton.PointerPressed += (s, e) => {
                UpdateSelectedButton(AdminButton);
                NavigateToPage(new Pages.Admin.RoleManagementPage());
            };
            
            ThemeButton.PointerPressed += (s, e) => {
                ToggleTheme();
            };
            
            ExitButton.PointerPressed += (s, e) => {
                Environment.Exit(0);
            };
            
            LogoutButton.PointerPressed += (s, e) => {
                AuthService.Logout(); // Use the public property
                NavigateToLogin();
            };
            
            // Обробка натискання кнопки GitHub
            GitHubButton.PointerPressed += (s, e) => {
                OpenGitHubRepo();
            };
        }
        
        // Обробка наведення курсору на sidebar - розширення
        private void Sidebar_PointerEnter(object? sender, PointerEventArgs e)
        {
            if (sender is Grid grid)
            {
                grid.Classes.Add("Expanded");
            }
        }
        
        // Обробка виходу курсору з sidebar - згортання
        private void Sidebar_PointerLeave(object? sender, PointerEventArgs e)
        {
            Debug.WriteLine("Sidebar: Pointer exited");
            if (sender is Grid grid)
            {
                grid.Classes.Remove("Expanded");
            }
        }

        private void UpdateSelectedButton(Border? newSelectedButton)
        {
            if (newSelectedButton == null) return;

            if (currentSelectedButton != null)
                currentSelectedButton.Classes.Remove("Selected");
                
            newSelectedButton.Classes.Add("Selected");
            currentSelectedButton = newSelectedButton;
        }

        private void NavigateToPage(Control page)
        {
            Border? contentBorder = this.FindControl<Border>("ContentBorder");
            if (contentBorder != null)
            {
                contentBorder.Child = page;
            }
        }
        
        // Публічний метод для навігації, який використовується іншими класами
        public void NavigateToPagePublic(Control page)
        {
            NavigateToPage(page);
        }

        private void ToggleTheme()
        {
            Grid? mainGrid = this.FindControl<Grid>("MainGrid");
            if (mainGrid != null)
            {
                ThemeManager.ToggleTheme(mainGrid);
            }
            UpdateThemeUI();
        }

        private void UpdateThemeUI()
        {
            Border? themeButton = this.FindControl<Border>("ThemeButton");
            if (themeButton != null)
            {
                Border? iconContainer = themeButton.GetVisualDescendants()
                    .OfType<Border>()
                    .FirstOrDefault(b => b.Classes.Contains("ButtonIconBackground"));

                TextBlock? iconTextBlock = iconContainer?.Child as TextBlock;

                TextBlock? labelTextBlock = themeButton.GetVisualDescendants()
                    .OfType<TextBlock>()
                    .FirstOrDefault(t => t.Classes.Contains("ButtonLabel"));
                
                if (iconTextBlock != null && labelTextBlock != null)
                {
                    if (ThemeManager.IsDarkTheme)
                    {
                        iconTextBlock.Text = "☀️";
                        labelTextBlock.Text = "Light Theme";
                    }
                    else
                    {
                        iconTextBlock.Text = "🌙";
                        labelTextBlock.Text = "Dark Theme";
                    }
                }
            }

        }
        
        // Метод для навігації до сторінки входу при виході з системи
        public void NavigateToLogin()
        {
            // Create and show a new LoginMenu window
            Auth.LoginMenu loginWindow = new Auth.LoginMenu();
            loginWindow.Show();
            
            // Close the current window
            this.Close();
        }

        private void OpenGitHubRepo()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/NotionMe/SortProgram",
                UseShellExecute = true
            });
        }
    }
}
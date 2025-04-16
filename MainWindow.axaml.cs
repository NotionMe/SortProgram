using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.Pages;
using Avalonia.Styling;
using Avalonia.Media;
using Avalonia;
using System.Diagnostics;
using Practika2_OPAM_Ubohyi_Stanislav.Styles;
using Avalonia.Input;
using System.Linq;
using Avalonia.VisualTree;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav
{
    public partial class SortProgram : Window
    {
        private Border? currentSelectedButton;
        public bool IsDarkTheme => ThemeManager.IsDarkTheme;

        public SortProgram()
        {
            InitializeComponent();
            
            // Initialize the language manager with default language
            // You can get system language or use saved user preference
            LanguageManager.Instance.LoadLanguage("en");

            NavigateToPage(new HomePage());
            UpdateSelectedButton(HomeButton);
            
            SetupEventHandlers();
            
            var mainGrid = this.FindControl<Grid>("MainGrid");
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
            
            ThemeButton.PointerPressed += (s, e) => {
                ToggleTheme();
            };
            
            ExitButton.PointerPressed += (s, e) => {
                Environment.Exit(0);
            };

            ProfileButton.PointerPressed += (s, e) => {
                UpdateSelectedButton(ProfileButton);
                NavigateToPage(new ProfilePage());
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
            var contentBorder = this.FindControl<Border>("ContentBorder");
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
            var mainGrid = this.FindControl<Grid>("MainGrid");
            if (mainGrid != null)
            {
                ThemeManager.ToggleTheme(mainGrid);
            }
            UpdateThemeUI();
        }

        private void UpdateThemeUI()
        {
            var themeButton = this.FindControl<Border>("ThemeButton");
            if (themeButton != null)
            {
                var iconContainer = themeButton.GetVisualDescendants()
                    .OfType<Border>()
                    .FirstOrDefault(b => b.Classes.Contains("ButtonIconBackground"));
                
                var iconTextBlock = iconContainer?.Child as TextBlock;
                
                var labelTextBlock = themeButton.GetVisualDescendants()
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
            NavigateToPage(new Auth.LoginMenu());
        }
    }
}
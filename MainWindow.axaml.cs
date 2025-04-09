using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.Pages;
using Avalonia.Styling;
using Avalonia.Media;
using Avalonia;
using System.Diagnostics;

namespace Practika2_OPAM_Ubohyi_Stanislav
{
    public partial class SortProgram : Window
    {
        private Button? currentSelectedButton;
        private bool isDarkTheme = false; // Default to light theme
        public bool IsDarkTheme => isDarkTheme;

        public SortProgram()
        {
            InitializeComponent();
            // Встановлюємо початкову сторінку та вибраний пункт меню
            NavigateToPage(new HomePage());
            UpdateSelectedButton(HomeButton);
            
            // Set initial theme to light
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = ThemeVariant.Light;
            }
        }

        private void UpdateSelectedButton(Button? newSelectedButton)
        {
            if (newSelectedButton == null) return;

            currentSelectedButton?.Classes.Remove("selected");
            newSelectedButton.Classes.Add("selected");
            currentSelectedButton = newSelectedButton;
        }

        private void NavigateToPage(Control page)
        {
            var pageContent = this.FindControl<ContentControl>("PageContent");
            if (pageContent != null)
            {
                pageContent.Content = page;
            }
        }

        private void MinimizeWindow(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeWindow(object? sender, RoutedEventArgs e) => 
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        private void CloseWindow(object? sender, RoutedEventArgs e) => Environment.Exit(0);

        private void NavigateToHome(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                UpdateSelectedButton(button);
                NavigateToPage(new HomePage());
            }
        }

        private void NavigateToSorting(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                UpdateSelectedButton(button);
                NavigateToPage(new SortingAlgorithmsPage());
            }
        }

        private void NavigateToStaticks(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                UpdateSelectedButton(button);
                NavigateToPage(new StatisticsPage());
            }
        }

        private void NavigateToSettings(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                UpdateSelectedButton(button);
                NavigateToPage(new SettingsPage());
            }
        }

        public void ExitButton_Click(object? sender, RoutedEventArgs e)
        {
            LoginWindow.LoginWindow loginWindow = new LoginWindow.LoginWindow();
            loginWindow.Show();
            this.Hide();
        }

        private void ToggleTheme(object? sender, RoutedEventArgs e)
        {
            isDarkTheme = !isDarkTheme;
            ApplyTheme();
        }

        public void ToggleThemeFromSettings(bool isDark)
        {
            if (isDarkTheme != isDark)
            {
                isDarkTheme = isDark;
                ApplyTheme();
            }
        }

        private void ApplyTheme()
        {
            // Update the application theme
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = isDarkTheme ? ThemeVariant.Dark : ThemeVariant.Light;
            }
            
            // Update the theme button icon and text
            var themeIcon = this.FindControl<TextBlock>("ThemeIcon");
            var themeText = this.FindControl<TextBlock>("ThemeText");
            
            if (themeIcon != null && themeText != null)
            {
                if (isDarkTheme)
                {
                    themeIcon.Text = "🌙";
                    themeText.Text = "Dark Theme";
                }
                else
                {
                    themeIcon.Text = "☀️";
                    themeText.Text = "Light Theme";
                }
            }
            
            // Update the main content background using theme classes
            var mainContentGrid = this.FindControl<Grid>("MainContentGrid");
            if (mainContentGrid != null)
            {
                // Remove both theme classes first
                mainContentGrid.Classes.Remove("ThemeLight");
                mainContentGrid.Classes.Remove("ThemeDark");
                
                // Add the appropriate theme class
                if (isDarkTheme)
                {
                    mainContentGrid.Classes.Add("ThemeDark");
                }
                else
                {
                    mainContentGrid.Classes.Add("ThemeLight");
                }
            }

            Debug.WriteLine($"Theme changed to {(isDarkTheme ? "Dark" : "Light")}");
        }
    }
}
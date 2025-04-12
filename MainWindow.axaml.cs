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

namespace Practika2_OPAM_Ubohyi_Stanislav
{
    public partial class SortProgram : Window
    {
        private Button? currentSelectedButton;
        // Використовуємо ThemeManager для керування темою
        public bool IsDarkTheme => ThemeManager.IsDarkTheme;

        public SortProgram()
        {
            InitializeComponent();
            // Встановлюємо початкову сторінку та вибраний пункт меню
            NavigateToPage(new HomePage());
            UpdateSelectedButton(HomeButton);
            
            // Початкова ініціалізація темної теми замість світлої
            var mainContentGrid = this.FindControl<Grid>("MainContentGrid");
            if (mainContentGrid != null)
            {
                // Використовуємо темну тему за замовчуванням
                ThemeManager.SetLightTheme(mainContentGrid);
            }
            UpdateThemeUI();
            
            // Підключаємо події наведення миші для sidebar
            var sidebar = this.FindControl<Grid>("SidebarMenu");
            if (sidebar != null)
            {
                sidebar.PointerEntered += Sidebar_PointerEntered;
                sidebar.PointerExited += Sidebar_PointerExited;
            }
        }
        
        // Обробка наведення курсору на sidebar
        private void Sidebar_PointerEntered(object? sender, PointerEventArgs e)
        {
            Debug.WriteLine("Sidebar: Pointer entered");
            if (sender is Grid grid)
            {
                grid.Classes.Add("hover");
            }
        }
        
        // Обробка виходу курсору з sidebar
        private void Sidebar_PointerExited(object? sender, PointerEventArgs e)
        {
            Debug.WriteLine("Sidebar: Pointer exited");
            if (sender is Grid grid)
            {
                grid.Classes.Remove("hover");
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
            Environment.Exit(0);
        }

        private void ToggleTheme(object? sender, RoutedEventArgs e)
        {
            var mainContentGrid = this.FindControl<Grid>("MainContentGrid");
            if (mainContentGrid != null)
            {
                ThemeManager.ToggleTheme(mainContentGrid);
            }
            UpdateThemeUI();
        }

        private void UpdateThemeUI()
        {
            // Оновлюємо іконку та текст кнопки теми
            var themeIcon = this.FindControl<TextBlock>("ThemeIcon");
            var themeText = this.FindControl<TextBlock>("ThemeText");
            
            if (themeIcon != null && themeText != null)
            {
                if (ThemeManager.IsDarkTheme)
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

            Debug.WriteLine($"Theme changed to {(ThemeManager.IsDarkTheme ? "Dark" : "Light")}");
        }

        // Публічний метод для навігації, який можна викликати з інших класів
        public void NavigateToPagePublic(Control page)
        {
            NavigateToPage(page);
        }
    }
}
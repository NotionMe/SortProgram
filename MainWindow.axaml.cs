using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.Pages;

namespace Practika2_OPAM_Ubohyi_Stanislav
{
    public partial class SortProgram : Window
    {
        private Button? currentSelectedButton;

        public SortProgram()
        {
            InitializeComponent();
            HomeButton?.Classes.Remove("selected");
            NavigateToPage(new HomePage());
            UpdateSelectedButton(HomeButton);
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
    }
}
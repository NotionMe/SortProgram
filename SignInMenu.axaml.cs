using Avalonia;
using Avalonia.Markup.Xaml;
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Practika2_OPAM_Ubohyi_Stanislav
{
    public partial class SignInMenu : Window
    {
        public SignInMenu()
        {
            InitializeComponent();
        }

        private void SignInButton_Click(object? sender, RoutedEventArgs e)
        {
            SortProgram mainWindow = new SortProgram();
            mainWindow.Show();
            this.Close();
        }

        private void LoginButton_Click(object? sender, RoutedEventArgs e)
        {
            LoginMenu loginMenu = new LoginMenu();
            loginMenu.Show();
            this.Close();
        }

    }
}
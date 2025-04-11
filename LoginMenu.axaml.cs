using Avalonia;
using Avalonia.Markup.Xaml;
using System;

using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Practika2_OPAM_Ubohyi_Stanislav
{
    public partial class LoginMenu : Window
    {
        public LoginMenu()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object? sender, RoutedEventArgs e)
        {
            SortProgram mainWindow = new SortProgram();
            mainWindow.Show();
            this.Close();
        }

        private void SignUpButton_Click(object? sender, RoutedEventArgs e)
        {
            SignInMenu signInMenu = new SignInMenu();
            signInMenu.Show();
            this.Close();
        }
    }
}
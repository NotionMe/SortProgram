using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.Pages;

namespace Practika2_OPAM_Ubohyi_Stanislav.LoginWindow;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }


    // Method to handle minimizing the window
    private void MinimizeWindow(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    // Method to handle maximizing/restoring the window
    private void MaximizeWindow(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    // Method to handle closing the window
    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    private void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        
        SortProgram sortProgram = new SortProgram();
        sortProgram.Show();
        this.Hide();
    }

    private void TogglePasswordButton_Click(object sender, RoutedEventArgs e)
    {
        if (PasswordTextBox.PasswordChar == '\0')
        {
            PasswordTextBox.PasswordChar = '•';
            TogglePasswordButton.Content = "👁️";
        }
        else
        {
            PasswordTextBox.PasswordChar = '\0';
            TogglePasswordButton.Content = "🙈";
        }
    }
}

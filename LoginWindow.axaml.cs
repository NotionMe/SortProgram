using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.LoginWindow
{
    public partial class LoginWindow : Window
    {
        private readonly TextBox? _passwordBox;
        private readonly TextBox? _emailBox;

        public LoginWindow()
        {
            InitializeComponent();
            _passwordBox = this.FindControl<TextBox>("PasswordBox");
            _emailBox = this.FindControl<TextBox>("EmailBox");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnTogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            var PasswordBox = this.FindControl<TextBox>("PasswordBox");
            var togglePasswordButton = this.FindControl<Button>("ShowPasswordButton"); 

            if (PasswordBox != null && togglePasswordButton != null)
            {
                if (PasswordBox.PasswordChar == '\0')
                {
                    PasswordBox.PasswordChar = '•';
                    togglePasswordButton.Content = "👁️";
                }
                else
                {
                    PasswordBox.PasswordChar = '\0';
                    togglePasswordButton.Content = "🙈";
                }
            }
        }

        private void CloseWindow(object? sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LoginButton_Click(object? sender, RoutedEventArgs e)
        {
            // Тут можна додати перевірку email
            if (_emailBox != null && !string.IsNullOrEmpty(_emailBox.Text) && !_emailBox.Text.Contains("@"))
            {
                // Можна додати повідомлення про помилку, якщо потрібно
                return;
            }
            
            SortProgram mainWindow = new SortProgram();
            mainWindow.Show();
            this.Close();
        }
    }
}

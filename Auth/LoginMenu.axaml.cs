using Avalonia;
using Avalonia.Markup.Xaml;
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Data;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav.Auth
{
    public partial class LoginMenu : Window
    {
        private TextBox? _emailTextBox;
        private TextBlock? _emailValidationText;
        private TextBox? _passwordTextBox;
        private TextBlock? _passwordValidationText;
        private readonly AuthService _authService;
        
        public LoginMenu()
        {
            InitializeComponent();
            
            _emailTextBox = this.FindControl<TextBox>("EmailTextBox");
            _emailValidationText = this.FindControl<TextBlock>("EmailValidationText");
            _passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            _passwordValidationText = this.FindControl<TextBlock>("PasswordValidationText");
            
            // Get AuthService instance
            _authService = AuthService.Instance;
            
            // Check initial state
            if (_emailTextBox != null)
            {
                ValidateEmail(_emailTextBox.Text);
            }
            
            if (_passwordTextBox != null)
            {
                ValidatePassword(_passwordTextBox.Text);
            }
        }

        private void EmailTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                ValidateEmail(textBox.Text);
            }
        }
        
        private void PasswordTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                ValidatePassword(textBox.Text);
            }
        }
        
        private void ValidateEmail(string? email)
        {
            if (_emailValidationText != null)
            {
                bool isEmpty = string.IsNullOrWhiteSpace(email);
                _emailValidationText.IsVisible = isEmpty;
                
                if (isEmpty)
                {
                    _emailValidationText.Text = "Email cannot be empty";
                }
                
            }
        }
        
        private void ValidatePassword(string? password)
        {
            if (_passwordValidationText != null)
            {
                bool isEmpty = string.IsNullOrWhiteSpace(password);
                _passwordValidationText.IsVisible = isEmpty;
                
                if (isEmpty)
                {
                    _passwordValidationText.Text = "Password cannot be empty";
                }
                else
                {
                    bool isValidLength = IsValidPassword(password);
                    _passwordValidationText.IsVisible = !isValidLength;
                    _passwordValidationText.Text = isValidLength ? "" : "Password must be at least 6 characters long";
                }
            }
        }

        private void LoginButton_Click(object? sender, RoutedEventArgs e)
        {
            // Validate all fields before proceeding
            bool isValid = true;
            
            if (_emailTextBox != null && string.IsNullOrWhiteSpace(_emailTextBox.Text))
            {
                ValidateEmail(_emailTextBox.Text);
                isValid = false;
            }
            
            if (_passwordTextBox != null && string.IsNullOrWhiteSpace(_passwordTextBox.Text))
            {
                ValidatePassword(_passwordTextBox.Text);
                isValid = false;
            }
            
            if (!isValid)
            {
                return;
            }

            try
            {
                // Use the AuthService to attempt login
                bool loginSuccess = _authService.LoginUser(
                    _emailTextBox?.Text ?? string.Empty,
                    _passwordTextBox?.Text ?? string.Empty
                );
                
                if (!loginSuccess)
                {
                    ShowErrorMessage("Login failed", "Invalid email/username or password");
                    return;
                }
                
                // Login successful, open main window
                var mainWindow = new Practika2_OPAM_Ubohyi_Stanislav.SortProgram();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Login error", $"An error occurred: {ex.Message}");
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
                
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }

        private bool IsValidPassword(string? password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
        }

        private void SignUpButton_Click(object? sender, RoutedEventArgs e)
        {
            SignInMenu signInMenu = new SignInMenu();
            signInMenu.Show();
            this.Close();
        }

        private void ShowErrorMessage(string title, string message)
        {
            var messageBox = new MessageBox
            {
                MessageTitle = title,
                Message = message
            };
            messageBox.ShowDialog(this);
        }
    }
}
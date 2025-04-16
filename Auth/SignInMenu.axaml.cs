using Avalonia;
using Avalonia.Markup.Xaml;
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Data;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav.Auth
{
    public partial class SignInMenu : Window
    {
        private TextBox? _usernameTextBox;
        private TextBlock? _usernameValidationText;
        private TextBox? _emailTextBox;
        private TextBlock? _emailValidationText;
        private TextBox? _passwordTextBox;
        private TextBlock? _passwordValidationText;
        private TextBox? _confirmPasswordTextBox;
        private TextBlock? _confirmPasswordValidationText;
        private readonly AuthService _authService;

        public SignInMenu()
        {
            InitializeComponent();
            
            _usernameTextBox = this.FindControl<TextBox>("UsernameTextBox");
            _usernameValidationText = this.FindControl<TextBlock>("UsernameValidationText");
            _emailTextBox = this.FindControl<TextBox>("EmailTextBox");
            _emailValidationText = this.FindControl<TextBlock>("EmailValidationText");
            _passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            _passwordValidationText = this.FindControl<TextBlock>("PasswordValidationText");
            _confirmPasswordTextBox = this.FindControl<TextBox>("ConfirmPasswordTextBox");
            _confirmPasswordValidationText = this.FindControl<TextBlock>("ConfirmPasswordValidationText");
            
            _authService = AuthService.Instance;
            
            // Check initial state for all fields
            ValidateField(_usernameTextBox?.Text, _usernameValidationText);
            ValidateField(_emailTextBox?.Text, _emailValidationText);
            ValidateField(_passwordTextBox?.Text, _passwordValidationText);
            ValidateField(_confirmPasswordTextBox?.Text, _confirmPasswordValidationText);
        }

        private void UsernameTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                ValidateField(textBox.Text, _usernameValidationText);
            }
        }
        
        private void EmailTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                ValidateField(textBox.Text, _emailValidationText);
            }
        }
        
        private void PasswordTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                ValidateField(textBox.Text, _passwordValidationText);
            }
        }
        
        private void ConfirmPasswordTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                ValidateField(textBox.Text, _confirmPasswordValidationText);
            }
        }
        
        private void ValidateField(string? value, TextBlock? validationText)
        {
            if (validationText != null)
            {
                bool isEmpty = string.IsNullOrWhiteSpace(value);
                validationText.IsVisible = isEmpty;
                
                if (!isEmpty)
                {
                    if (validationText == _emailValidationText)
                    {
                        bool isValidFormat = IsValidEmail(value!);
                        validationText.IsVisible = !isValidFormat;
                        validationText.Text = isValidFormat ? "" : "Invalid email format";
                    }
                    else if (validationText == _passwordValidationText)
                    {
                        bool isValidLength = IsValidPassword(value);
                        validationText.IsVisible = !isValidLength;
                        validationText.Text = isValidLength ? "" : "Password must be at least 6 characters long";
                    }
                    else if (validationText == _confirmPasswordValidationText && _passwordTextBox != null)
                    {
                        bool passwordsMatch = _passwordTextBox.Text == value;
                        validationText.IsVisible = !passwordsMatch;
                        validationText.Text = passwordsMatch ? "" : "Passwords don't match";
                    }
                }
                else
                {
                    if (validationText == _usernameValidationText)
                        validationText.Text = "Username cannot be empty";
                    else if (validationText == _emailValidationText)
                        validationText.Text = "Email cannot be empty";
                    else if (validationText == _passwordValidationText)
                        validationText.Text = "Password cannot be empty";
                    else if (validationText == _confirmPasswordValidationText)
                        validationText.Text = "Please confirm your password";
                }
            }
        }

        private void SignInButton_Click(object? sender, RoutedEventArgs e)
        {
            bool isValid = true;
            
            if (string.IsNullOrWhiteSpace(_usernameTextBox?.Text))
            {
                ValidateField(_usernameTextBox?.Text, _usernameValidationText);
                isValid = false;
            }
            
            if (string.IsNullOrWhiteSpace(_emailTextBox?.Text))
            {
                ValidateField(_emailTextBox?.Text, _emailValidationText);
                isValid = false;
            }
            
            if (string.IsNullOrWhiteSpace(_passwordTextBox?.Text))
            {
                ValidateField(_passwordTextBox?.Text, _passwordValidationText);
                isValid = false;
            }
            
            if (string.IsNullOrWhiteSpace(_confirmPasswordTextBox?.Text))
            {
                ValidateField(_confirmPasswordTextBox?.Text, _confirmPasswordValidationText);
                isValid = false;
            }
            
            if (!isValid)
            {
                return;
            }

            try
            {
                // Use AuthService to register user
                bool registrationSuccess = _authService.RegisterUser(
                    _usernameTextBox?.Text ?? string.Empty,
                    _emailTextBox?.Text ?? string.Empty,
                    _passwordTextBox?.Text ?? string.Empty
                );
                
                if (!registrationSuccess)
                {
                    ShowErrorMessage("Registration error", "Username or email already exists!");
                    return;
                }
                
                // Open main window
                var mainWindow = new Practika2_OPAM_Ubohyi_Stanislav.SortProgram();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Registration error", $"Failed to register: {ex.Message}");
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

        private void LoginButton_Click(object? sender, RoutedEventArgs e)
        {
            LoginMenu loginMenu = new LoginMenu();
            loginMenu.Show();
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
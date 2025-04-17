using Avalonia;
using Avalonia.Markup.Xaml;
using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Data;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav.Auth
{
    /// <summary>
    /// Вікно входу, що забезпечує автентифікацію користувача через електронну пошту/ім'я користувача та пароль.
    /// Надає функції валідації введення та автентифікації.
    /// </summary>
    public partial class LoginMenu : Window
    {
        private TextBox? _emailTextBox;
        private TextBlock? _emailValidationText;
        private TextBox? _passwordTextBox;
        private TextBlock? _passwordValidationText;
        private readonly AuthService _authService;
        
        /// <summary>
        /// Ініціалізує форму входу та налаштовує валідацію полів введення
        /// </summary>
        public LoginMenu()
        {
            InitializeComponent();
            
            _emailTextBox = this.FindControl<TextBox>("EmailTextBox");
            _emailValidationText = this.FindControl<TextBlock>("EmailValidationText");
            _passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            _passwordValidationText = this.FindControl<TextBlock>("PasswordValidationText");
            
            // Отримуємо екземпляр AuthService
            _authService = AuthService.Instance;
            
            // Перевіряємо початковий стан
            if (_emailTextBox != null)
            {
                ValidateEmailOrUsername(_emailTextBox.Text);
            }
            
            if (_passwordTextBox != null)
            {
                ValidatePassword(_passwordTextBox.Text);
            }
        }

        /// <summary>
        /// Обробник події зміни поля електронної пошти/імені користувача, що запускає валідацію
        /// </summary>
        private void EmailTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                ValidateEmailOrUsername(textBox.Text);
            }
        }
        
        /// <summary>
        /// Обробник події зміни поля пароля, що запускає валідацію
        /// </summary>
        private void PasswordTextBox_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TextBox.TextProperty && sender is TextBox textBox)
            {
                ValidatePassword(textBox.Text);
            }
        }
        
        /// <summary>
        /// Валідує поле електронної пошти або імені користувача
        /// Приймає або валідний формат електронної пошти, або будь-яке непорожнє ім'я користувача
        /// </summary>
        private void ValidateEmailOrUsername(string? input)
        {
            if (_emailValidationText != null)
            {
                bool isEmpty = string.IsNullOrWhiteSpace(input);
                _emailValidationText.IsVisible = isEmpty;
                
                if (isEmpty)
                {
                    _emailValidationText.Text = "Email/Username not be empty";
                }
                else
                {
                    // Приймаємо або валідний формат електронної пошти, або ім'я користувача (без додаткової валідації)
                    _emailValidationText.IsVisible = false;
                }
            }
        }
        
        /// <summary>
        /// Валідує поле введення пароля
        /// Відображає повідомлення про помилку для порожніх паролів або тих, що мають недостатню довжину
        /// </summary>
        private void ValidatePassword(string? password)
        {
            if (_passwordValidationText != null)
            {
                bool isEmpty = string.IsNullOrWhiteSpace(password);
                _passwordValidationText.IsVisible = isEmpty;
                
                if (isEmpty)
                {
                    _passwordValidationText.Text = "Password not be empty";
                }
                else
                {
                    bool isValidLength = IsValidPassword(password);
                    _passwordValidationText.IsVisible = !isValidLength;
                    _passwordValidationText.Text = isValidLength ? "" : "Password must be at least 6 characters long";
                }
            }
        }

        /// <summary>
        /// Обробляє спробу входу при натисканні кнопки входу
        /// Валідує всі введені дані, намагається здійснити автентифікацію та керує відгуком інтерфейсу
        /// </summary>
        private void LoginButton_Click(object? sender, RoutedEventArgs e)
        {
            // Валідуємо всі поля перед продовженням
            bool isValid = true;
            
            if (string.IsNullOrWhiteSpace(_emailTextBox?.Text))
            {
                ValidateEmailOrUsername(_emailTextBox?.Text);
                isValid = false;
            }
            
            if (string.IsNullOrWhiteSpace(_passwordTextBox?.Text) || !IsValidPassword(_passwordTextBox?.Text))
            {
                ValidatePassword(_passwordTextBox?.Text);
                isValid = false;
            }
            
            if (!isValid)
            {
                return;
            }

            try
            {
                // Автентифікуємо користувача з наданими обліковими даними
                bool loginSuccess = _authService.LoginUser(
                    _emailTextBox?.Text ?? string.Empty,
                    _passwordTextBox?.Text ?? string.Empty
                );
                
                if (!loginSuccess)
                {
                    ShowErrorMessage("Login failed", "Invalid email/username or password");
                    return;
                }
                
                // Вхід успішний, відкриваємо головне вікно
                var mainWindow = new SortProgram();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                // Обробка помилок автентифікації
                ShowErrorMessage("Login error", $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Перевіряє формат електронної пошти за допомогою регулярного виразу
        /// </summary>
        /// <returns>True, якщо формат електронної пошти валідний</returns>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
                
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// Перевіряє вимоги до пароля (мінімум 6 символів)
        /// </summary>
        /// <returns>True, якщо пароль відповідає мінімальним вимогам</returns>
        private bool IsValidPassword(string? password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
        }

        /// <summary>
        /// Перенаправляє до екрану реєстрації при натисканні кнопки реєстрації
        /// </summary>
        private void SignUpButton_Click(object? sender, RoutedEventArgs e)
        {
            SignInMenu signInMenu = new SignInMenu();
            signInMenu.Show();
            this.Close();
        }

        /// <summary>
        /// Відображає повідомлення про помилки користувачу за допомогою спеціального діалогу MessageBox
        /// </summary>
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
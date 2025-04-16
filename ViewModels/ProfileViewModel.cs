using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;
using System.Windows.Input;
using Avalonia.Controls;
using System;
using Avalonia;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.Pages;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private string _userName = string.Empty;
        private string _email = string.Empty;
        private DateTime _registrationDate;
        private readonly AuthService _authService;

        public new LanguageManager LanguageManager => LanguageManager.Instance;

        // Команди для кнопок
        public ICommand EditProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public ProfileViewModel()
        {
            _authService = AuthService.Instance;
            UpdateUserInfo();
            LanguageManager.Instance.LanguageChanged += (s, e) => this.RaisePropertyChanged(nameof(LanguageManager));

            // Ініціалізація команд
            EditProfileCommand = ReactiveCommand.Create(EditProfile);
            LogoutCommand = ReactiveCommand.Create(Logout);
        }

        private void UpdateUserInfo()
        {
            var currentUser = _authService.GetCurrentUser();
            UserName = currentUser.Username ?? "Ім'я користувача";
            Email = currentUser.Email ?? "user@email.com";
            
            // Отримуємо дату реєстрації або встановлюємо поточну, якщо вона не встановлена
            if (currentUser.RegistrationDate != default)
            {
                RegistrationDate = currentUser.RegistrationDate;
            }
            else
            {
                // Якщо дати реєстрації немає, встановлюємо поточну дату
                RegistrationDate = DateTime.Now;
                currentUser.RegistrationDate = RegistrationDate;
                _authService.SetCurrentUser(currentUser);
            }
        }

        // Метод для редагування профілю
        private void EditProfile()
        {
            // Створюємо вікно для редагування профілю
            var window = new Window
            {
                Title = "Редагування профілю",
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            var stackPanel = new StackPanel
            {
                Spacing = 10,
                Margin = new Thickness(20)
            };

            var usernameTextBox = new TextBox
            {
                Watermark = "Ім'я користувача",
                Text = UserName
            };

            var emailTextBox = new TextBox
            {
                Watermark = "Email",
                Text = Email
            };

            var saveButton = new Button
            {
                Content = "Зберегти",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Width = 100,
                Height = 40
            };

            saveButton.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(usernameTextBox.Text) && !string.IsNullOrWhiteSpace(emailTextBox.Text))
                {
                    // Оновлення даних користувача
                    var currentUser = _authService.GetCurrentUser();
                    currentUser.Username = usernameTextBox.Text;
                    currentUser.Email = emailTextBox.Text;
                    _authService.SetCurrentUser(currentUser);

                    // Оновлюємо відображення
                    UpdateUserInfo();
                    window.Close();
                }
            };

            stackPanel.Children.Add(new TextBlock { Text = "Редагування профілю", FontSize = 20, FontWeight = Avalonia.Media.FontWeight.Bold });
            stackPanel.Children.Add(usernameTextBox);
            stackPanel.Children.Add(emailTextBox);
            stackPanel.Children.Add(saveButton);

            window.Content = stackPanel;
            window.Show();
        }

        // Метод для виходу з системи
        private void Logout()
        {
            // Очистити інформацію про користувача
            _authService.Logout();
            
            // Перенаправити на сторінку входу
            var mainWindow = Avalonia.Application.Current?.ApplicationLifetime as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime;
            var window = mainWindow?.MainWindow as Practika2_OPAM_Ubohyi_Stanislav.SortProgram;
            window?.NavigateToLogin();
        }
        
        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }
        
        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set => this.RaiseAndSetIfChanged(ref _registrationDate, value);
        }
    }
}

using ReactiveUI;
using System.Windows.Input;
using Avalonia.Controls;
using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using System.Reactive.Linq;
using Avalonia.Threading;
using Avalonia.ReactiveUI;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Concurrency;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private string _userName = string.Empty;
        private string _email = string.Empty;
        private DateTime _registrationDate;
        private string _userRole = string.Empty;
        private string _avatarPath = string.Empty;
        private Bitmap? _avatarImage;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IAvatarService _avatarService;
        private bool _isEditingUsername;
        private bool _isEditingEmail;
        private string _tempUsername = string.Empty;
        private string _tempEmail = string.Empty;

        // Validation properties
        private string _usernameValidationMessage = string.Empty;
        private string _emailValidationMessage = string.Empty;
        private bool _isUsernameValid = true;
        private bool _isEmailValid = true;

        public new LanguageManager LanguageManager => LanguageManager.Instance;

        public bool IsEditingUsername
        {
            get => _isEditingUsername;
            set => this.RaiseAndSetIfChanged(ref _isEditingUsername, value);
        }

        public bool IsEditingEmail
        {
            get => _isEditingEmail;
            set => this.RaiseAndSetIfChanged(ref _isEditingEmail, value);
        }

        public string UsernameValidationMessage
        {
            get => _usernameValidationMessage;
            set => this.RaiseAndSetIfChanged(ref _usernameValidationMessage, value);
        }

        public string EmailValidationMessage
        {
            get => _emailValidationMessage;
            set => this.RaiseAndSetIfChanged(ref _emailValidationMessage, value);
        }

        public bool IsUsernameValid
        {
            get => _isUsernameValid;
            set => this.RaiseAndSetIfChanged(ref _isUsernameValid, value);
        }

        public bool IsEmailValid
        {
            get => _isEmailValid;
            set => this.RaiseAndSetIfChanged(ref _isEmailValid, value);
        }

        public ICommand ToggleUsernameEditCommand { get; }
        public ICommand ToggleEmailEditCommand { get; }
        public ICommand SaveUsernameCommand { get; }
        public ICommand SaveEmailCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand ChangeAvatarCommand { get; }
        public ICommand DeleteAccountCommand { get; }

        public ProfileViewModel()
        {
            _authService = App.GetService<IAuthService>();
            _userRepository = App.GetService<IUserRepository>();
            _avatarService = App.GetService<IAvatarService>();

            UpdateUserInfo();
            LanguageManager.Instance.LanguageChanged += (s, e) =>
                Dispatcher.UIThread.Post(() => this.RaisePropertyChanged(nameof(LanguageManager)));

            // Create canExecute observables on the UI thread
            var canToggleUsername = this.WhenAnyValue(x => x.IsEditingEmail)
                .Select(isEditingEmail => !isEditingEmail)
                .ObserveOn(AvaloniaScheduler.Instance);

            var canToggleEmail = this.WhenAnyValue(x => x.IsEditingUsername)
                .Select(isEditingUsername => !isEditingUsername)
                .ObserveOn(AvaloniaScheduler.Instance);

            // Observe property changes for validation
            this.WhenAnyValue(x => x.UserName)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(ValidateUsername);

            this.WhenAnyValue(x => x.Email)
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(ValidateEmail);

            // Create commands with the UI thread scheduler
            ToggleUsernameEditCommand = ReactiveCommand.Create(ToggleUsernameEdit, canToggleUsername, AvaloniaScheduler.Instance);
            ToggleEmailEditCommand = ReactiveCommand.Create(ToggleEmailEdit, canToggleEmail, AvaloniaScheduler.Instance);
            SaveUsernameCommand = ReactiveCommand.Create(SaveUsername, outputScheduler: AvaloniaScheduler.Instance);
            SaveEmailCommand = ReactiveCommand.Create(SaveEmail, outputScheduler: AvaloniaScheduler.Instance);
            CancelEditCommand = ReactiveCommand.Create(CancelEdit, outputScheduler: AvaloniaScheduler.Instance);
            ChangeAvatarCommand = ReactiveCommand.CreateFromTask(ChangeAvatarAsync, outputScheduler: AvaloniaScheduler.Instance);
            DeleteAccountCommand = ReactiveCommand.Create(DeleteAccount, outputScheduler: AvaloniaScheduler.Instance);
        }

        // Validation methods
        private void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                IsUsernameValid = false;
                UsernameValidationMessage = "Ім'я користувача не може бути порожнім";
                return;
            }

            if (username.Length < 3)
            {
                IsUsernameValid = false;
                UsernameValidationMessage = "Ім'я користувача повинно мати не менше 3 символів";
                return;
            }

            var currentUser = _authService.GetCurrentUser();
            var users = _userRepository.GetAllUsers();

            // Check if username already exists (excluding current user)
            bool usernameExists = users.Any(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Email != currentUser.Email);

            if (usernameExists)
            {
                IsUsernameValid = false;
                UsernameValidationMessage = "Це ім'я користувача вже зайнято";
                return;
            }

            IsUsernameValid = true;
            UsernameValidationMessage = string.Empty;
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                IsEmailValid = false;
                EmailValidationMessage = "Email не може бути порожнім";
                return;
            }

            // Regex for email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
            {
                IsEmailValid = false;
                EmailValidationMessage = "Неправильний формат email";
                return;
            }

            var currentUser = _authService.GetCurrentUser();
            var users = _userRepository.GetAllUsers();

            // Check if email already exists (excluding current user)
            bool emailExists = users.Any(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Username != currentUser.Username);

            if (emailExists)
            {
                IsEmailValid = false;
                EmailValidationMessage = "Цей email вже використовується";
                return;
            }

            IsEmailValid = true;
            EmailValidationMessage = string.Empty;
        }

        private void ToggleUsernameEdit()
        {
            if (!IsEditingUsername)
            {
                _tempUsername = UserName;
                IsUsernameValid = true;
                UsernameValidationMessage = string.Empty;
                IsEditingUsername = true;
            }
        }

        private void ToggleEmailEdit()
        {
            if (!IsEditingEmail)
            {
                _tempEmail = Email;
                IsEmailValid = true;
                EmailValidationMessage = string.Empty;
                IsEditingEmail = true;
            }
        }

        private void SaveUsername()
        {
            ValidateUsername(UserName);

            var currentUser = _authService.GetCurrentUser();
            if (currentUser != null && !string.IsNullOrWhiteSpace(UserName) && IsUsernameValid)
            {
                currentUser.Username = UserName;
                _authService.UpdateCurrentUser(currentUser);
                IsEditingUsername = false;
            }
        }

        private void SaveEmail()
        {
            ValidateEmail(Email);

            var currentUser = _authService.GetCurrentUser();
            if (currentUser != null && !string.IsNullOrWhiteSpace(Email) && IsEmailValid)
            {
                currentUser.Email = Email;
                _authService.UpdateCurrentUser(currentUser);
                IsEditingEmail = false;
            }
        }

        private void CancelEdit()
        {
            if (IsEditingUsername)
            {
                UserName = _tempUsername;
                IsUsernameValid = true;
                UsernameValidationMessage = string.Empty;
                IsEditingUsername = false;
            }
            if (IsEditingEmail)
            {
                Email = _tempEmail;
                IsEmailValid = true;
                EmailValidationMessage = string.Empty;
                IsEditingEmail = false;
            }
        }

        private async System.Threading.Tasks.Task ChangeAvatarAsync()
        {
            try
            {
                var dialog = new AvatarPickerDialog();

                // Set initial selected avatar if user already has one
                var user = _authService.GetCurrentUser();
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    // Pre-select the current avatar
                    dialog.PreSelectAvatar(user.Avatar);
                }

                // Використовуємо інший підхід до отримання батьківського вікна
                Window? parentWindow = null;

                // Виконуємо пошук батьківського вікна на UI потоці
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    // Шукаємо активне вікно серед відкритих вікон додатку
                    if (Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        foreach (Window window in desktop.Windows)
                        {
                            if (window.IsActive)
                            {
                                parentWindow = window;
                                break;
                            }
                        }

                        // Якщо активне вікно не знайдено, використовуємо перше з відкритих вікон
                        if (parentWindow == null && desktop.Windows.Count > 0)
                        {
                            parentWindow = desktop.Windows[0];
                        }
                    }
                });

                if (parentWindow == null)
                {
                    Console.WriteLine("Failed to find valid parent window for avatar dialog");
                    return;
                }

                // Перевіряємо, чи дійсне батьківське вікно, перш ніж показувати діалог
                if (!parentWindow.IsVisible)
                {
                    Console.WriteLine("Parent window is not visible, cannot show dialog");
                    return;
                }

                var result = await dialog.ShowDialog<bool?>(parentWindow);

                if (result == true)
                {
                    string newAvatarPath = dialog.SelectedAvatarPath;

                    // Make sure we have a valid path
                    if (string.IsNullOrEmpty(newAvatarPath))
                    {
                        Console.WriteLine("No avatar selected");
                        return;
                    }

                    // Update user avatar
                    user.Avatar = newAvatarPath;
                    _authService.UpdateCurrentUser(user);

                    // Update UI
                    AvatarPath = newAvatarPath;
                    AvatarImage = _avatarService.LoadAvatar(newAvatarPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing avatar: {ex.Message}");
            }
        }
        private void UpdateUserInfo()
        {
            var currentUser = _authService.GetCurrentUser();
            UserName = currentUser.Username ?? "Ім'я користувача";
            Email = currentUser.Email ?? "user@email.com";
            UserRole = currentUser.Role;

            // Set avatar path
            string avatarPath = string.IsNullOrEmpty(currentUser.Avatar) ?
                "avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Images/Avatar/Avatar1.png" :
                currentUser.Avatar;

            AvatarPath = avatarPath;

            try
            {
                // Load the avatar image
                AvatarImage = _avatarService.LoadAvatar(avatarPath);
                Console.WriteLine($"Avatar loaded successfully from {avatarPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load avatar: {ex.Message}");
                // Try to load a fallback image
                try
                {
                    AvatarImage = _avatarService.LoadAvatar("avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Images/Avatar/Avatar1.png");
                }
                catch
                {
                    // If even the fallback fails, leave it null
                    Console.WriteLine("Failed to load fallback avatar as well");
                }
            }

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

        public string UserRole
        {
            get => _userRole;
            set => this.RaiseAndSetIfChanged(ref _userRole, value);
        }

        public string AvatarPath
        {
            get => _avatarPath;
            set => this.RaiseAndSetIfChanged(ref _avatarPath, value);
        }

        public Bitmap? AvatarImage
        {
            get => _avatarImage;
            set => this.RaiseAndSetIfChanged(ref _avatarImage, value);
        }

        private async void DeleteAccount()
        {
            // Показуємо діалогове вікно підтвердження
            bool confirmed = await AccountDelete.MessageDelete.ShowAsync(   
                LanguageManager.GetString("DeleteProfileText"),
                LanguageManager.GetString("Delete_profile"));

            // Видаляємо акаунт тільки якщо користувач підтвердив дію
            if (confirmed)
            {
                DeleteUserAccount();
            }
        }

        private void DeleteUserAccount()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser != null)
                {
                    // Видаляємо користувача з бази даних
                    var allUsers = _userRepository.GetAllUsers();
                    allUsers.RemoveAll(u => u.Email == currentUser.Email);

                    // Зберігаємо оновлений список користувачів
                    SaveAllUsers(allUsers);

                    // Виходимо з облікового запису
                    _authService.Logout();

                    // Перенаправляємо на сторінку входу
                    NavigateToLoginPage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user account: {ex.Message}");
            }
        }

        private void SaveAllUsers(List<User> users)
        {
            try
            {
                var tempRepository = new UserRepository();

                System.IO.File.WriteAllText("Assets/DataBase/users.json", "[]");

                foreach (var user in users)
                {
                    tempRepository.SaveUser(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }

        private void NavigateToLoginPage()
        {
            LoginMenu loginMenu = new LoginMenu();
            loginMenu.Show();

            if (Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (Window window in desktop.Windows)
                {
                    if (window is SortProgram)
                    {
                        window.Close();
                        break;
                    }
                }
            }
        }
    }
}

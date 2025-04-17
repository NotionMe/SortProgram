using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;
using System.Windows.Input;
using Avalonia.Controls;
using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.IO;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.Pages;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia.Threading;
using Avalonia.ReactiveUI;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Avalonia.Platform.Storage;

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
        private readonly AuthService _authService;
        private readonly UserRepository _userRepository;
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

        public ProfileViewModel()
        {
            _authService = AuthService.Instance;
            _userRepository = new UserRepository();
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
                AvatarImage = LoadFromResource(avatarPath);
                Console.WriteLine($"Avatar loaded successfully from {avatarPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load avatar: {ex.Message}");
                // Try to load a fallback image
                try
                {
                    AvatarImage = LoadFromResource("avares://Practika2_OPAM_Ubohyi_Stanislav/Assets/Images/Avatar/Avatar1.png");
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

        private Bitmap? LoadFromResource(string uri)
        {
            if (string.IsNullOrEmpty(uri))
                return null;

            try
            {
                // Use AssetLoader as a static class
                using var stream = AssetLoader.Open(new Uri(uri));
                return new Bitmap(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image from {uri}: {ex.Message}");

                // Fallback to file-based loading
                try
                {
                    if (uri.StartsWith("avares://"))
                    {
                        string filePath = uri.Replace("avares://Practika2_OPAM_Ubohyi_Stanislav/", "");
                        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

                        if (File.Exists(fullPath))
                        {
                            using var fileStream = File.OpenRead(fullPath);
                            return new Bitmap(fileStream);
                        }
                    }
                }
                catch (Exception fileEx)
                {
                    Console.WriteLine($"Error loading image from file: {fileEx.Message}");
                }
            }
            return null;
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
    }
}

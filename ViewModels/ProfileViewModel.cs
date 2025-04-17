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

        public new LanguageManager LanguageManager => LanguageManager.Instance;

        public ICommand EditProfileCommand { get; }

        public ProfileViewModel()
        {
            _authService = AuthService.Instance;
            UpdateUserInfo();
            LanguageManager.Instance.LanguageChanged += (s, e) => this.RaisePropertyChanged(nameof(LanguageManager));

            EditProfileCommand = ReactiveCommand.Create(EditProfile);
        }

        private void EditProfile()
        {
            // Тут буде логіка редагування профілю
            // Наприклад, відкриття діалогового вікна для редагування
            Console.WriteLine("Редагування профілю");
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

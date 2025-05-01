using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav.Auth
{
    public partial class AvatarPickerDialog : Window
    {
        private readonly IAvatarService _avatarService;
        private string _selectedAvatarPath = string.Empty;
        
        public string SelectedAvatarPath => _selectedAvatarPath;
        public ObservableCollection<AvatarItem> Avatars { get; } = new ObservableCollection<AvatarItem>();

        public AvatarPickerDialog()
        {
            _avatarService = App.GetService<IAvatarService>();
            InitializeComponent();
            DataContext = this;
            Opened += AvatarPickerDialog_Opened;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void AvatarPickerDialog_Opened(object? sender, EventArgs e)
        {
            LoadAvatars();
        }

        private void LoadAvatars()
        {
            try
            {
                Avatars.Clear();
                List<string> avatarPaths = _avatarService.GetAllAvatarPaths();
                
                // Keep track if we found any valid avatar
                bool foundValidAvatar = false;
                
                foreach (string path in avatarPaths)
                {
                    Bitmap? image = _avatarService.LoadAvatar(path);
                    if (image != null)
                    {
                        Avatars.Add(new AvatarItem 
                        { 
                            Path = path, 
                            Image = image,
                            IsSelected = false
                        });
                        foundValidAvatar = true;
                    }
                    else
                    {
                        // Log error but continue with other avatars
                        Console.WriteLine($"Failed to load avatar from path: {path}");
                    }
                }
                
                // Select first avatar by default if we haven't preselected one
                if (Avatars.Count > 0 && string.IsNullOrEmpty(_selectedAvatarPath))
                {
                    _selectedAvatarPath = Avatars[0].Path;
                    Avatars[0].IsSelected = true;
                }
                else if (!foundValidAvatar)
                {
                    // No valid avatars were found
                    Console.WriteLine("No valid avatars were found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading avatars: {ex.Message}");
            }
        }

        private void AvatarItem_Clicked(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is AvatarItem item)
            {
                try
                {
                    // Unselect all avatars
                    foreach (var avatar in Avatars)
                    {
                        avatar.IsSelected = false;
                    }
                    
                    // Select clicked avatar
                    item.IsSelected = true;
                    _selectedAvatarPath = item.Path;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error selecting avatar: {ex.Message}");
                }
            }
        }

        private void OkButton_Click(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedAvatarPath))
            {
                // Ensure we have a selected avatar path
                if (Avatars.Count > 0)
                {
                    var selected = Avatars.FirstOrDefault(a => a.IsSelected);
                    if (selected != null)
                    {
                        _selectedAvatarPath = selected.Path;
                    }
                    else
                    {
                        _selectedAvatarPath = Avatars[0].Path;
                    }
                }
            }
            
            Close(true);
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }
        
        public void PreSelectAvatar(string avatarPath)
        {
            if (string.IsNullOrEmpty(avatarPath))
                return;
                
            _selectedAvatarPath = avatarPath;
            
            // If avatars are already loaded, select the matching one
            if (Avatars.Count > 0)
            {
                // First deselect all
                foreach (var avatar in Avatars)
                {
                    avatar.IsSelected = false;
                }
                
                // Find and select the matching one
                var matchingAvatar = Avatars.FirstOrDefault(a => a.Path == avatarPath);
                if (matchingAvatar != null)
                {
                    matchingAvatar.IsSelected = true;
                }
                else
                {
                    // If no match found, select the first one
                    Avatars[0].IsSelected = true;
                    _selectedAvatarPath = Avatars[0].Path;
                }
            }
        }
    }
    
    public class AvatarItem : INotifyPropertyChanged
    {
        private string _path = string.Empty;
        private Bitmap? _image;
        private bool _isSelected;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }
        
        public Bitmap? Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
    }
}
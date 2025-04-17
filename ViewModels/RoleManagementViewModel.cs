using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Threading;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using ReactiveUI;
using System.Linq;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class RoleManagementViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
        private readonly UserRepository _userRepository;
        private UserViewModel? _selectedUserViewModel;
        private string _selectedRole;
        private ObservableCollection<UserViewModel> _userViewModels;
        private List<string> _availableRoles;

        public RoleManagementViewModel()
        {
            _authService = AuthService.Instance;
            _userRepository = new UserRepository();
            List<User> users = _userRepository.GetAllUsers();
            _userViewModels = new ObservableCollection<UserViewModel>(
                users.Select(u => new UserViewModel(u)));
            _availableRoles = _userRepository.GetAvailableRoles();
            _selectedRole = string.Empty;

            UpdateRoleCommand = new RelayCommand(UpdateRole, CanUpdateRole);
            
            // Check if current user is admin
            if (!_authService.IsAdmin())
            {
                // Handle unauthorized access
                // Could show a message or redirect
            }
        }

        public ObservableCollection<UserViewModel> UserViewModels => _userViewModels;

        public List<string> AvailableRoles => _availableRoles;

        public UserViewModel? SelectedUserViewModel
        {
            get => _selectedUserViewModel;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedUserViewModel, value);
                if (_selectedUserViewModel != null)
                {
                    SelectedRole = _selectedUserViewModel.Role;
                }
                ((RelayCommand)UpdateRoleCommand).RaiseCanExecuteChanged();
            }
        }

        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedRole, value);
                ((RelayCommand)UpdateRoleCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand UpdateRoleCommand { get; }

        private bool CanUpdateRole(object? parameter)
        {
            return _selectedUserViewModel != null && 
                   !string.IsNullOrEmpty(_selectedRole) && 
                   _selectedUserViewModel.Role != _selectedRole;
        }

        private void UpdateRole(object? parameter)
        {
            if (SelectedUserViewModel == null || string.IsNullOrEmpty(SelectedRole))
                return;

            // Handle case where user tries to modify their own role
            if (SelectedUserViewModel.Username == _authService.GetCurrentUser().Username)
            {
                // Show error message or handle appropriately
                return;
            }
                
            if (_authService.ChangeUserRole(SelectedUserViewModel.Username, SelectedRole))
            {
                // Update the user's role in our local collection
                SelectedUserViewModel.Role = SelectedRole;
                
                // Refresh the list
                RefreshUsers();
            }
        }

        private void RefreshUsers()
        {
            List<User> users = _userRepository.GetAllUsers();
            
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _userViewModels.Clear();
                foreach (User user in users)
                {
                    _userViewModels.Add(new UserViewModel(user));
                }
            });
        }

        // Method to handle user selection for the code-behind approach
        public void SelectUser(UserViewModel userViewModel, string role)
        {
            SelectedUserViewModel = userViewModel;
            SelectedRole = role;
        }
    }

    // Relay Command implementation if not already in your project
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

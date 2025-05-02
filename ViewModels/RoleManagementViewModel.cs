using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Threading;
using ReactiveUI;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using Practika2_OPAM_Ubohyi_Stanislav.Services;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class RoleManagementViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleService _roleService;
        private UserViewModel? _selectedUserViewModel;
        private string _selectedRole;
        private ObservableCollection<UserViewModel> _userViewModels;
        private List<string> _availableRoles;

        public RoleManagementViewModel()
        {
            _authService = App.GetService<IAuthService>();
            _userRepository = App.GetService<IUserRepository>();
            _roleService = App.GetService<IRoleService>();
            
            List<User> users = _userRepository.GetAllUsers();
            _userViewModels = new ObservableCollection<UserViewModel>(
                users.Select(u => new UserViewModel(u)));
            _availableRoles = _roleService.GetAvailableRoles();
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
                
            if (_roleService.ChangeUserRole(SelectedUserViewModel.Username, SelectedRole))
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
            // Ensure CanExecute is always evaluated on UI thread
            if (!Dispatcher.UIThread.CheckAccess())
            {
                bool result = false;
                Dispatcher.UIThread.InvokeAsync(() => 
                {
                    result = _canExecute == null || _canExecute(parameter);
                }, DispatcherPriority.Send).Wait();
                return result;
            }
            
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            // Always execute on UI thread
            if (!Dispatcher.UIThread.CheckAccess())
            {
                Dispatcher.UIThread.InvokeAsync(() => _execute(parameter));
                return;
            }
            
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            // Ensure the event is raised on the UI thread
            if (!Dispatcher.UIThread.CheckAccess())
            {
                Dispatcher.UIThread.InvokeAsync(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
                return;
            }
            
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

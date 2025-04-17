using Practika2_OPAM_Ubohyi_Stanislav.Auth;
using ReactiveUI;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private User _user;

        public UserViewModel(User user)
        {
            _user = user;
        }

        public string Username
        {
            get => _user.Username;
            set
            {
                _user.Username = value;
                this.RaisePropertyChanged(nameof(Username));
            }
        }

        public string Email
        {
            get => _user.Email;
            set
            {
                _user.Email = value;
                this.RaisePropertyChanged(nameof(Email));
            }
        }

        public string Role
        {
            get => _user.Role;
            set
            {
                _user.Role = value;
                this.RaisePropertyChanged(nameof(Role));
            }
        }

        public User User => _user;
    }
}
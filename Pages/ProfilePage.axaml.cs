using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class ProfilePage : UserControl
    {
        public ProfilePage()
        {
            InitializeComponent();
            DataContext = new ProfileViewModel();
        }
    }
}

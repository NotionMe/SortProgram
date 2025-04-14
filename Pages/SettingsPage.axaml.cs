using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
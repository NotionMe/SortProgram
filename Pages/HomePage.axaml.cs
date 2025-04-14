using Avalonia;
using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations;
using System.Threading.Tasks;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = new HomePageViewModel();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
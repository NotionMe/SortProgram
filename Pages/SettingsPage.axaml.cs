using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class SettingsPage : UserControl
    {
        private SettingsViewModel _viewModel;

        public SettingsPage()
        {
            InitializeComponent();
            
            _viewModel = new SettingsViewModel();
            DataContext = _viewModel;
            
            // Find the main content grid when the control is loaded
            this.AttachedToVisualTree += (sender, e) => 
            {
                Window? window = this.VisualRoot as Window;
                if (window != null)
                {
                    Grid? mainGrid = window.FindControl<Grid>("MainGrid");
                    if (mainGrid != null)
                    {
                        _viewModel.Initialize(mainGrid);
                    }
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
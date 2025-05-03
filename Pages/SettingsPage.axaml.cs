using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.InfoAlgoritm;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

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

        private void OpenTutorial_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new Algoritm());
            }
        }

        private void NavigateToPage(Control page)
        {
            Border? contentBorder = this.FindControl<Border>("ContentBorder");
            if (contentBorder != null)
            {
                contentBorder.Child = page;
            }
        }
        
        public void NavigateToPagePublic(Control page)
        {
            NavigateToPage(page);
        }
    }
}
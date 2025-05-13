using Avalonia.Controls;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels; // Added for ViewModel
using Avalonia.Markup.Xaml; // Keep for InitializeComponent
using Practika2_OPAM_Ubohyi_Stanislav.Pages; // For SortingAlgorithmsPage if used for navigation
using System.Diagnostics;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers; // For Debug.WriteLine

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.VisualSearching
{
    public partial class JumpSearch : UserControl
    {
        public JumpSearchViewModel ViewModel { get; private set; } 

        public JumpSearch()
        {
            InitializeComponent(); // Added InitializeComponent call
            ViewModel = new JumpSearchViewModel(); // Initialize ViewModel
            DataContext = ViewModel;  // Set DataContext
        }

        private void InitializeComponent() // Added InitializeComponent method
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new SortingAlgorithmsPage());

        }
    }
}




using Avalonia.Controls;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels; // Added for ViewModel
using Avalonia.Markup.Xaml; // Keep for InitializeComponent
using Practika2_OPAM_Ubohyi_Stanislav.Pages; // For SortingAlgorithmsPage if used for navigation
using System.Diagnostics; // For Debug.WriteLine

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.VisualSearching
{
    public partial class LinearSearch : UserControl
    {
        public LinearSearchViewModel ViewModel { get; private set; }

        public LinearSearch()
        {
            InitializeComponent();
            ViewModel = new LinearSearchViewModel();
            DataContext = ViewModel; 
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

         private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            // Повернення до списку алгоритмів
            if (this.VisualRoot is SortProgram mainWindow)
            {
                mainWindow.NavigateToPagePublic(new SortingAlgorithmsPage());
            }
        }
    }
}




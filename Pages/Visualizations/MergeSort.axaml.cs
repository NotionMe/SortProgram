using Avalonia.Controls;
using Avalonia.Interactivity;

using System.ComponentModel;


namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations
{
    public partial class MergeSort : UserControl, INotifyPropertyChanged
    {
        public MergeSort()
        {
            InitializeComponent();
            
           
        }

      
        private void GenerateButton_Click(object? sender, RoutedEventArgs e)
        {
            
        }
        
        private void StartButton_Click(object? sender, RoutedEventArgs e)
        {
            // You can also add media control functions here
        }

        private void PauseButton_Click(object? sender, RoutedEventArgs e)
        {
        }

        private void StepButton_Click(object? sender, RoutedEventArgs e)
        {
        }

        private void ResetButton_Click(object? sender, RoutedEventArgs e)
        {
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



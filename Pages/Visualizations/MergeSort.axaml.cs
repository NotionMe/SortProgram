using Avalonia.Controls;
using Avalonia.Interactivity;
using System.ComponentModel;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations
{
    public partial class MergeSort : UserControl
    {

        public MergeSort()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object? sender, RoutedEventArgs e)
        {
        }

        private void PauseButton_Click(object? sender, RoutedEventArgs e)
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



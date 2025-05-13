using Avalonia.Controls;
using Avalonia.Interactivity;
using System.ComponentModel;
using System;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers;

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
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new SortingAlgorithmsPage());

        }
    }
}



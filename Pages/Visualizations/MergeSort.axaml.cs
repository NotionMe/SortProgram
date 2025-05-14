using Avalonia.Controls;
using Avalonia.Interactivity;
using System.ComponentModel;
using System;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers;
using Practika2_OPAM_Ubohyi_Stanislav.ErrorMessage;
using DynamicData.Kernel;

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
            ErrorMessage.ErrorMessage.ShowError();
        }

        private void PauseButton_Click(object? sender, RoutedEventArgs e)
        {
            ErrorMessage.ErrorMessage.ShowError();
        }
        
        private void ResetButton_Click(object? sender, RoutedEventArgs e)
        {
            ErrorMessage.ErrorMessage.ShowError();
        }

        private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new SortingAlgorithmsPage());

        }
    }
}



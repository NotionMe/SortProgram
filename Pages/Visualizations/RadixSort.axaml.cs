using Avalonia.Controls;
using Avalonia.Interactivity;
using System.ComponentModel;
using System;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations
{
    public partial class RadixSort : UserControl
    {

        public RadixSort()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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



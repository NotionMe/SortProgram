using Avalonia.Controls;
using Avalonia.Interactivity;
using System.ComponentModel;
using System;
using Avalonia.Markup.Xaml;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations
{
    public partial class HeapSort : UserControl
    {

        public HeapSort()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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



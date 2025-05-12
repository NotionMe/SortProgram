using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.GraphTraversal;

public partial class BFSPage : UserControl
{
    public BFSPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void GenerateButton_Click(object? sender, RoutedEventArgs e)
        {
        }

        private void StartButton_Click(object? sender, RoutedEventArgs e)
        {
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

using System;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers;
using Avalonia.Interactivity;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.TreebasedAlgorithms;


public partial class Preorder_TraversalPage : UserControl
{
    public Preorder_TraversalPage()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void GenerateButton_Click(object? sender, RoutedEventArgs e)
        {
            ErrorMessage.ErrorMessage.ShowError();
        }

        private void StartButton_Click(object? sender, RoutedEventArgs e)
        {
            ErrorMessage.ErrorMessage.ShowError();
        }

        private void PauseButton_Click(object? sender, RoutedEventArgs e)
        {
            ErrorMessage.ErrorMessage.ShowError();
        }

        private void StepButton_Click(object? sender, RoutedEventArgs e)
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
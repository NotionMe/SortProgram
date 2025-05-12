using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info.GraphTraversal;

public partial class InfoBFS : UserControl
{
    public InfoBFS()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AttemptButton_Click(object sender, RoutedEventArgs e)
    {
        SortProgram? mainWindow = this.VisualRoot as SortProgram;
        if (mainWindow != null)
        {
            //mainWindow.NavigateToPagePublic(new BFSPage());
        }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        SortProgram? mainWindow = this.VisualRoot as SortProgram;

        if (mainWindow != null)
        {
            SortingAlgorithmsPage algorithmsPage = new SortingAlgorithmsPage();
            mainWindow.NavigateToPagePublic(algorithmsPage);
        }
        else
        {
            if (this.Parent is ContentControl contentControl)
            {
                contentControl.Content = new SortingAlgorithmsPage();
            }
        }
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        SortProgram? mainWindow = this.VisualRoot as SortProgram;
        if (mainWindow != null)
        {
            mainWindow.NavigateToPagePublic(new InfoSelectionSort());
        }
    }

    private void NotesButton_Click(object sender, RoutedEventArgs e)
    {
        SortProgram? mainWindow = this.VisualRoot as SortProgram;
        if (mainWindow != null && mainWindow.AuthService != null)
        {
            Notate notateWindow = new Notate(mainWindow.AuthService);
            notateWindow.Show();

            notateWindow.SortComboBox.SelectedIndex = 10;
            notateWindow.LoadNoteForSelectedSort();
        }
        else
        {
            Console.WriteLine("Main window or AuthService is null.");
        }
    }
}

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.GraphTraversal;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.TreebasedAlgorithms;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info.GraphTraversal;

public partial class InfoInorder_Traversal : UserControl
{
    public InfoInorder_Traversal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AttemptButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new Inorder_TraversalPage());
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new SortingAlgorithmsPage());
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoPreorder_Traversal());

    }

    private void NotesButton_Click(object sender, RoutedEventArgs e)
    {
        SortProgram? mainWindow = this.VisualRoot as SortProgram;
        if (mainWindow != null && mainWindow.AuthService != null)
        {
            Notate notateWindow = new Notate(mainWindow.AuthService);
            notateWindow.Show();

            notateWindow.SortComboBox.SelectedIndex = 13;
            notateWindow.LoadNoteForSelectedSort();
        }
        else
        {
            Console.WriteLine("Main window or AuthService is null.");
        }
    }
}

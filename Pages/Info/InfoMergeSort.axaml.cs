using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info;

public partial class InfoMergeSort : UserControl
{
    public InfoMergeSort()
    {
        InitializeComponent();
    }
        private void AttemptButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new MergeSort());
        }
    private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new SortingAlgorithmsPage());
        }
        
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoHeapSort());
        }
        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null && mainWindow.AuthService != null)
            {
                Notate notateWindow = new Notate(mainWindow.AuthService);
                notateWindow.Show();
                
                notateWindow.SortComboBox.SelectedIndex = 4; 
                notateWindow.LoadNoteForSelectedSort(); 
            }
            else
            {
                Console.WriteLine("Main window or AuthService is null.");
            }
        }
}
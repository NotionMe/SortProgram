using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.Algorithms;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info;
  

public partial class InfoHeapSort : UserControl
{
    public InfoHeapSort()
    {
        InitializeComponent();
    }
            private void AttemptButton_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new HeapSort());
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
                mainWindow.NavigateToPagePublic(new InfoRadixSort());
            }
        }
        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null && mainWindow.AuthService != null)
            {
                Notate notateWindow = new Notate(mainWindow.AuthService);
                notateWindow.Show();
                
                notateWindow.SortComboBox.SelectedIndex = 5; 
                notateWindow.LoadNoteForSelectedSort(); 
            }
            else
            {
                Console.WriteLine("Main window or AuthService is null.");
            }
        }
}
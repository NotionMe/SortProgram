using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info;

public partial class InfoRadixSort : UserControl
{
    public InfoRadixSort()
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
                mainWindow.NavigateToPagePublic(new RadixSort());
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
        // SortProgram? mainWindow = this.VisualRoot as SortProgram;
        // if (mainWindow != null)
        // {
        //     mainWindow.NavigateToPagePublic(new InfoSelectionSort());
        // }
    }
    private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null && mainWindow.AuthService != null)
            {
                Notate notateWindow = new Notate(mainWindow.AuthService);
                notateWindow.Show();
                
                notateWindow.SortComboBox.SelectedIndex = 6; 
                notateWindow.LoadNoteForSelectedSort(); 
            }
            else
            {
                System.Console.WriteLine("Main window or AuthService is null.");
            }
        }
}
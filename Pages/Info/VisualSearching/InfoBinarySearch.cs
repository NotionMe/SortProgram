using System;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Notates;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.VisualSearching;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info.VisualSearching
{
    public partial class InfoBinarySearch : UserControl
    {
        public InfoBinarySearch()
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
                mainWindow.NavigateToPagePublic(new BinarySearch());
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
                mainWindow.NavigateToPagePublic(new InfoJumpSearch());
            }
        }
        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null && mainWindow.AuthService != null)
            {
                Notate notateWindow = new Notate(mainWindow.AuthService);
                notateWindow.Show();

                notateWindow.SortComboBox.SelectedIndex = 8;
                notateWindow.LoadNoteForSelectedSort();
            }
            else
            {
                Console.WriteLine("Main window or AuthService is null.");
            }
        }
    }
}
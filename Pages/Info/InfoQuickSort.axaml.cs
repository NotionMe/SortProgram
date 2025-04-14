using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Info
{
    public partial class InfoQuickSort : UserControl
    {
        public InfoQuickSort()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as Practika2_OPAM_Ubohyi_Stanislav.SortProgram;
            
            if (mainWindow != null)
            {
                var algorithmsPage = new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage();
                mainWindow.NavigateToPagePublic(algorithmsPage);
            }
            else
            {
                if (this.Parent is ContentControl contentControl)
                {
                    contentControl.Content = new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage();
                }
            }
        }
        
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as Practika2_OPAM_Ubohyi_Stanislav.SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoInsertionSort());
            }
        }
    }
}

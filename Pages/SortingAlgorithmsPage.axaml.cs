using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Info;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public partial class SortingAlgorithmsPage : UserControl
    {
        public SortingAlgorithmsPage()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void InfoBubbleSort_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoBubbleSort());
            }
        }

        private void BubbleSortPage_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new BubbleSort());
            }
        }

        private void SelectionSortPage_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.SelectionSort());
            }
        }

        private void QuickSortPage_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new Practika2_OPAM_Ubohyi_Stanislav.Pages.Info.InfoQuickSort());
            }
        }

        private void InfoSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoSelectionSort());
            }
        }

        private void InfoQuickSort_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new Practika2_OPAM_Ubohyi_Stanislav.Pages.Info.InfoQuickSort());
            }
        }

        private void InfoInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoInsertionSort());
            }
        }

        private void InsertionSortPage_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                // Тимчасово перенаправляємо на інформаційну сторінку, поки не реалізована візуалізація
                mainWindow.NavigateToPagePublic(new InfoInsertionSort());
            }
        }
    }
}

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Info;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations;
using System;
using System.Linq;
using Avalonia.Media;
using System.Collections.Generic;
using Avalonia.Threading;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages
{
    public static class ControlExtensions
    {
        public static T? FindDescendantOfType<T>(this Control control, Func<T, bool>? predicate = null) where T : class
        {
            if (control == null)
                return null;
                
            if (control is T tcontrol && (predicate == null || predicate(tcontrol)))
                return tcontrol;
                
            foreach (var child in control.GetVisualChildren())
            {
                if (child is Control childControl)
                {
                    var result = childControl.FindDescendantOfType<T>(predicate);
                    if (result != null)
                        return result;
                }
            }
            
            return null;
        }
    }

    public partial class SortingAlgorithmsPage : UserControl
    {
        public SortingAlgorithmsPage()
        {
            InitializeComponent();

            Algoritm = this.FindControl<AutoCompleteBox>("Algoritm");

            if (Algoritm != null)
            {
                Algoritm.ItemsSource = _sortingAlgorithms.OrderBy(x => x);
            }
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private List<string> _sortingAlgorithms = new List<string>
        {
            "Bubble Sort",
            "Selection Sort",
            "Quick Sort",
            "Insertion Sort"
        };

        private void Algoritm_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = Algoritm?.Text?.ToLower() ?? string.Empty;
            
            var algorithmBorders = this.FindControl<StackPanel>("AlgorithmsContainer")?.Children
                .OfType<Border>().ToList();
            
            if (algorithmBorders == null || !algorithmBorders.Any())
                return;
                
            foreach (var border in algorithmBorders)
            {
                var textBlock = border.FindDescendantOfType<TextBlock>(tb => tb.FontWeight == FontWeight.SemiBold && tb.FontSize == 36);
                
                if (textBlock != null)
                {
                    string algorithmName = textBlock.Text?.ToLower() ?? string.Empty;
                    
                    border.IsVisible = string.IsNullOrEmpty(searchText) || 
                                    algorithmName.Contains(searchText);
                }
            }
        }

        private void InfoBubbleSort_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoBubbleSort());
            }
        }

        private void BubbleSortPage_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new BubbleSort());
            }
        }

        private void SelectionSortPage_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new SelectionSort());
            }
        }

        private void QuickSortPage_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new QuickSort());
            }
        }

        private void InfoSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoSelectionSort());
            }
        }

        private void InfoQuickSort_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoQuickSort());
            }
        }

        private void InfoInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InfoInsertionSort());
            }
        }

        private void InsertionSortPage_Click(object sender, RoutedEventArgs e)
        {
            SortProgram? mainWindow = this.VisualRoot as SortProgram;
            if (mainWindow != null)
            {
                mainWindow.NavigateToPagePublic(new InsertionSort());
            }
        }
    }
}
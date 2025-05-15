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
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Info.VisualSearching;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.VisualSearching;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Helpers;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.InfoAlgoritm;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Info.GraphTraversal;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.GraphTraversal;
using Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations.TreebasedAlgorithms;

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
            "Insertion Sort",
            "Merge Sort",
            "Heap Sort",
            "Radix Sort"
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
        private Dictionary<string, List<string>> _algorithmCategories = new Dictionary<string, List<string>>
        {
            { "Sorting", new List<string> { "Bubble Sort", "Selection Sort", "Quick Sort", "Insertion Sort", "Merge Sort", "Heap Sort", "Radix Sort", "Сортування бульбашкою",
            "Сортування вибором", "Швидке сортування", "Сортування вставками", "Сортування злиттям", "Пірамідальне сортування", "Сортування за розрядами" } },
            { "Searching", new List<string> { "Binary Search", "Linear Search", "Jump Search", "Лінійний пошук", "Бінарний пошук", "Стрибковий пошук" } },
            { "Graph Traversal", new List<string> { "Breadth-First Search", "Пошук у ширину", "Depth-First Search", "Пошук у глибину", "Dijkstras Algorithm", "Алгоритм Дейкстри"  } },
            { "Tree-based", new List<string> { "Inorder Traversal", "Cиметричний обхід", "Preorder Traversal", "Обхід у прямому порядку", "Postorder Traversal", "Обхід у зворотному порядку" } }
        };

        private void CategoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAlgorithmVisibility();
        }
        private void CategoryCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateAlgorithmVisibility();
        }

        private void UpdateAlgorithmVisibility()
        {
            List<string> selectedCategories = new List<string>();

            if (this.FindControl<CheckBox>("SortingCheckBox")?.IsChecked == true)
                selectedCategories.Add("Sorting");

            if (this.FindControl<CheckBox>("SearchingCheckBox")?.IsChecked == true)
                selectedCategories.Add("Searching");

            if (this.FindControl<CheckBox>("GraphCheckBox")?.IsChecked == true)
                selectedCategories.Add("Graph Traversal");

            if (this.FindControl<CheckBox>("TreeCheckBox")?.IsChecked == true)
                selectedCategories.Add("Tree-based");

            var algorithmBorders = this.FindControl<StackPanel>("AlgorithmsContainer")?.Children
                .OfType<Border>().ToList();
            if (algorithmBorders == null || !algorithmBorders.Any())
                return;

            if (!selectedCategories.Any())
            {
                foreach (var border in algorithmBorders)
                {
                    border.IsVisible = true;
                }
                return;
            }

            HashSet<string> visibleAlgorithms = new HashSet<string>();
            foreach (var category in selectedCategories)
            {
                if (_algorithmCategories.TryGetValue(category, out var algorithms))
                {
                    foreach (var algorithm in algorithms)
                    {
                        visibleAlgorithms.Add(algorithm);
                    }
                }
            }

            foreach (var border in algorithmBorders)
            {
                var textBlock = border.FindDescendantOfType<TextBlock>(tb => tb.FontWeight == FontWeight.SemiBold && tb.FontSize == 36);

                if (textBlock != null)
                {
                    string algorithmName = textBlock.Text ?? string.Empty;
                    border.IsVisible = visibleAlgorithms.Contains(algorithmName);
                }
            }

            if (Algoritm != null)
            {
                Algoritm.ItemsSource = visibleAlgorithms.Any()
                    ? visibleAlgorithms.OrderBy(x => x)
                    : _sortingAlgorithms.OrderBy(x => x);
            }
        }

        private void InfoBubbleSort_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoBubbleSort());
        }

        private void BubbleSortPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new BubbleSort());
        }

        private void SelectionSortPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new SelectionSort());
        }

        private void QuickSortPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new QuickSort());
        }

        private void InfoSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoSelectionSort());
        }

        private void InfoQuickSort_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoQuickSort());
        }

        private void InfoInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoInsertionSort());
        }

        private void InsertionSortPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InsertionSort());
        }

        private void InfoMergeSort_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoMergeSort());
        }

        private void MergeSortPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new MergeSort());
        }

        private void InfoHeapSort_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoHeapSort());
        }

        private void HeapSortPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new HeapSort());
        }

        private void InfoRadixSort_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoRadixSort());
        }

        private void RadixSortPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new RadixSort());
        }

        private void InfoLinearSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoLinearSearch());
        }

        private void LinearSearchPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new LinearSearch());
        }

        private void InfoBinarySearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoBinarySearch());
        }
        private void BinarySearchPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new BinarySearch());
        }

        private void InfoJumpSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoJumpSearch());
        }
        private void JumpSearchPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new JumpSearch());
        }
        private void InfoBFS_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoBFS());
        }
        private void BFSPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new BFSPage());
        }
        private void InfoDFS_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoDFS());
        }
        private void DFSPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new DFSPage());
        }
        private void InfoDijkstras_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoDijkstras());
        }
        private void DijkstrasPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new Dijkstars());
        }
        private void InfoInorder_Traversal_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoInorder_Traversal());
        }
        private void Inorder_TraversalPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new Inorder_TraversalPage());
        }
        private void InfoPreorder_Traversal_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoPreorder_Traversal());
        }
        private void Preorder_TraversalPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new Preorder_TraversalPage());
        }
        private void InfoPostorder_Traversal_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new InfoPostorder_Traversal());
        }
        private void Postorder_TraversalPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationHandlers.NavigateToPage(this.VisualRoot as SortProgram, new Postorder_TraversalPage());
        }
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations
{
    public partial class SelectionSort : UserControl
    {
        private Grid? _visualizationGrid;
        private TextBlock? _comparisonsTextBlock;
        private TextBlock? _swapsTextBlock;
        private TextBlock? _timeTextBlock;
        private ComboBox? _arraySizeComboBox;
        private ComboBox? _speedComboBox;
        private ComboBox? _arrayTypeComboBox;
        private SelectionSortViewModel? _viewModel;

        public SelectionSort()
        {
            InitializeComponent();

            this.AttachedToVisualTree += (s, e) =>
            {
                // Ініціалізація елементів після завантаження UI
                _visualizationGrid = this.FindControl<Grid>("VisualizationGrid");
                _comparisonsTextBlock = this.FindControl<TextBlock>("ComparisonsTextBlock");
                _swapsTextBlock = this.FindControl<TextBlock>("SwapsTextBlock");
                _timeTextBlock = this.FindControl<TextBlock>("TimeTextBlock");
                _arraySizeComboBox = this.FindControl<ComboBox>("ArraySizeComboBox");
                _speedComboBox = this.FindControl<ComboBox>("SpeedComboBox");
                _arrayTypeComboBox = this.FindControl<ComboBox>("ArrayTypeComboBox");

                if (_visualizationGrid != null)
                {
                    // Створення ViewModel та прив'язка до View
                    _viewModel = new SelectionSortViewModel(_visualizationGrid);
                    DataContext = _viewModel;
                    
                    // Налаштування прив'язок даних
                    SetupBindings();
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupBindings()
        {
            if (_viewModel == null) return;
            
            // Прив'язка ComboBox до властивостей ViewModel
            if (_arraySizeComboBox != null)
            {
                _arraySizeComboBox.SelectedIndex = _viewModel.SelectedArraySize;
                _arraySizeComboBox.SelectionChanged += (s, e) => {
                    if (_viewModel != null)
                        _viewModel.SelectedArraySize = _arraySizeComboBox.SelectedIndex;
                };
            }

            if (_speedComboBox != null)
            {
                _speedComboBox.SelectedIndex = _viewModel.SelectedSpeed;
                _speedComboBox.SelectionChanged += (s, e) => {
                    if (_viewModel != null)
                        _viewModel.SelectedSpeed = _speedComboBox.SelectedIndex;
                };
            }

            if (_arrayTypeComboBox != null)
            {
                _arrayTypeComboBox.SelectedIndex = _viewModel.SelectedArrayType;
                _arrayTypeComboBox.SelectionChanged += (s, e) => {
                    if (_viewModel != null)
                        _viewModel.SelectedArrayType = _arrayTypeComboBox.SelectedIndex;
                };
            }
            
            // Підписка на зміни властивостей ViewModel
            _viewModel.PropertyChanged += (s, e) => {
                if (e.PropertyName == nameof(_viewModel.Comparisons) && _comparisonsTextBlock != null)
                {
                    _comparisonsTextBlock.Text = _viewModel.Comparisons.ToString();
                }
                else if (e.PropertyName == nameof(_viewModel.Swaps) && _swapsTextBlock != null)
                {
                    _swapsTextBlock.Text = _viewModel.Swaps.ToString();
                }
                else if (e.PropertyName == nameof(_viewModel.ExecutionTime) && _timeTextBlock != null)
                {
                    _timeTextBlock.Text = _viewModel.ExecutionTime;
                }
            };
        }

        // Обробники подій кнопок
        private void GenerateButton_Click(object? sender, RoutedEventArgs e)
        {
            _viewModel?.GenerateCommand.Execute().Subscribe();
        }

        private void StartButton_Click(object? sender, RoutedEventArgs e)
        {
            _viewModel?.StartCommand.Execute().Subscribe();
        }

        private void PauseButton_Click(object? sender, RoutedEventArgs e)
        {
            _viewModel?.PauseCommand.Execute().Subscribe();
        }

        private void StepButton_Click(object? sender, RoutedEventArgs e)
        {
            _viewModel?.StepCommand.Execute().Subscribe();
        }

        private void ResetButton_Click(object? sender, RoutedEventArgs e)
        {
            _viewModel?.ResetCommand.Execute().Subscribe();
        }

        private void BackButton_Click(object? sender, RoutedEventArgs e)
        {
            // Повернення до списку алгоритмів
            if (this.VisualRoot is Practika2_OPAM_Ubohyi_Stanislav.SortProgram mainWindow)
            {
                mainWindow.NavigateToPagePublic(new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage());
            }
        }
    }
}

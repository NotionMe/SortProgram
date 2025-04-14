using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Practika2_OPAM_Ubohyi_Stanislav.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class BubbleSortViewModel : ViewModelBase
    {
        // Приватні поля
        private int[] _array = Array.Empty<int>();
        private DispatcherTimer? _timer;
        private int _i = 0, _j = 0;
        private bool _isSorting = false;
        private bool _isPaused = false;
        private int _comparisons = 0;
        private int _swaps = 0;
        private PerformanceTimer _performanceTimer = new PerformanceTimer();
        private readonly Grid _visualizationGrid;
        private int _selectedArraySize = 3; // Індекс 3 = 20 елементів
        private int _selectedSpeed = 2; // Індекс 2 = Середня швидкість
        private int _selectedArrayType = 0; // Індекс 0 = Випадковий масив
        private string _executionTime = "0 мс";
        private ObservableCollection<Border> _bars = new ObservableCollection<Border>();

        // Публічні властивості
        public ObservableCollection<Border> Bars
        {
            get => _bars;
            set => this.RaiseAndSetIfChanged(ref _bars, value);
        }

        public int SelectedArraySize
        {
            get => _selectedArraySize;
            set => this.RaiseAndSetIfChanged(ref _selectedArraySize, value);
        }

        public int SelectedSpeed
        {
            get => _selectedSpeed;
            set => this.RaiseAndSetIfChanged(ref _selectedSpeed, value);
        }

        public int SelectedArrayType
        {
            get => _selectedArrayType;
            set => this.RaiseAndSetIfChanged(ref _selectedArrayType, value);
        }

        public int Comparisons
        {
            get => _comparisons;
            set => this.RaiseAndSetIfChanged(ref _comparisons, value);
        }

        public int Swaps
        {
            get => _swaps;
            set => this.RaiseAndSetIfChanged(ref _swaps, value);
        }

        public string ExecutionTime
        {
            get => _executionTime;
            set => this.RaiseAndSetIfChanged(ref _executionTime, value);
        }

        public bool IsSorting
        {
            get => _isSorting;
            set => this.RaiseAndSetIfChanged(ref _isSorting, value);
        }

        public bool IsPaused
        {
            get => _isPaused;
            set => this.RaiseAndSetIfChanged(ref _isPaused, value);
        }

        // Команди
        public ReactiveCommand<Unit, Unit> GenerateCommand { get; }
        public ReactiveCommand<Unit, Unit> StartCommand { get; }
        public ReactiveCommand<Unit, Unit> PauseCommand { get; }
        public ReactiveCommand<Unit, Unit> StepCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }

        // Конструктор
        public BubbleSortViewModel(Grid visualizationGrid)
        {
            _visualizationGrid = visualizationGrid;
            
            // Ініціалізація команд
            GenerateCommand = ReactiveCommand.Create(GenerateArray);
            StartCommand = ReactiveCommand.Create(StartSorting);
            PauseCommand = ReactiveCommand.Create(PauseSorting);
            StepCommand = ReactiveCommand.Create(StepSorting);
            ResetCommand = ReactiveCommand.Create(ResetSorting);
            
            // Ініціалізація таймера
            InitializeTimer();
            
            // Генерація початкового масиву (відкладена до завантаження UI)
            _array = ArrayGenerator.GenerateRandomArray(GetArraySize());
            
            // Відкладаємо відображення масиву до завантаження UI
            Dispatcher.UIThread.Post(() => {
                if (_visualizationGrid.Bounds.Width > 0)
                {
                    DisplayArray();
                }
                else
                {
                    // Якщо розміри ще не встановлені, підписуємося на подію зміни розмірів
                    _visualizationGrid.LayoutUpdated += (s, e) => {
                        if (_visualizationGrid.Bounds.Width > 0)
                        {
                            DisplayArray();
                            // Відписуємося від події після першого виклику
                            _visualizationGrid.LayoutUpdated -= (s, e) => { };
                        }
                    };
                }
            }, DispatcherPriority.Loaded);
        }

        // Методи
        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GetSpeedInMilliseconds())
            };
            _timer.Tick += Timer_Tick;
        }

        private int GetSpeedInMilliseconds()
        {
            // Перетворення індексу швидкості на мілісекунди
            switch (_selectedSpeed)
            {
                case 0: return 1000; // Дуже повільно
                case 1: return 500;  // Повільно
                case 2: return 200;  // Середньо
                case 3: return 50;   // Швидко
                case 4: return 10;   // Дуже швидко
                default: return 200;
            }
        }

        private int GetArraySize()
        {
            // Перетворення індексу розміру масиву на фактичний розмір
            switch (_selectedArraySize)
            {
                case 0: return 5;
                case 1: return 10;
                case 2: return 15;
                case 3: return 20;
                case 4: return 25;
                case 5: return 30;
                case 6: return 40;
                case 7: return 50;
                default: return 20;
            }
        }

        public void GenerateArray()
        {
            // Скидання стану сортування
            ResetSortingState();

            // Генерація масиву відповідно до вибраного типу
            int size = GetArraySize();
            switch (_selectedArrayType)
            {
                case 0: // Випадковий
                    _array = ArrayGenerator.GenerateRandomArray(size);
                    break;
                case 1: // Майже відсортований
                    _array = ArrayGenerator.GenerateNearlySortedArray(size);
                    break;
                case 2: // Зворотній порядок
                    _array = ArrayGenerator.GenerateReversedArray(size);
                    break;
                case 3: // Мало унікальних елементів
                    _array = ArrayGenerator.GenerateFewUniqueArray(size);
                    break;
                default:
                    _array = ArrayGenerator.GenerateRandomArray(size);
                    break;
            }

            // Відображення масиву
            DisplayArray();
        }

        private void DisplayArray()
        {
            // Очищення візуалізації
            _visualizationGrid.Children.Clear();
            Bars.Clear();

            // Перевірка, чи розміри візуалізаційної сітки вже встановлені
            if (_visualizationGrid.Bounds.Width <= 0 || _array.Length == 0)
            {
                return;
            }

            // Знаходження максимального значення для масштабування
            int maxValue = _array.Max();
            if (maxValue == 0) maxValue = 1; // Уникаємо ділення на нуль
            
            // Створення візуального представлення для кожного елемента масиву
            double barWidth = Math.Max((_visualizationGrid.Bounds.Width / _array.Length) - 2, 1);
            double totalWidth = _visualizationGrid.Bounds.Width;
            double spacing = 2; // Відступ між стовпчиками
            
            // Очищаємо всі колонки
            _visualizationGrid.ColumnDefinitions.Clear();
            
            // Додаємо колонки для кожного елемента масиву
            for (int i = 0; i < _array.Length; i++)
            {
                _visualizationGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            }
            
            for (int i = 0; i < _array.Length; i++)
            {
                double barHeight = Math.Max((_array[i] / (double)maxValue) * _visualizationGrid.Bounds.Height * 0.9, 1);
                
                var bar = new Border
                {
                    Width = barWidth,
                    Height = barHeight,
                    Background = new SolidColorBrush(Color.Parse("#0000FF")), // Синій колір
                    Margin = new Thickness(spacing/2),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom
                };
                
                // Встановлюємо колонку для стовпчика
                Grid.SetColumn(bar, i);
                
                Bars.Add(bar);
                _visualizationGrid.Children.Add(bar);
            }
        }

        private void StartSorting()
        {
            if (IsSorting && IsPaused)
            {
                // Відновлення сортування після паузи
                IsPaused = false;
                _timer?.Start();
                return;
            }
            
            if (IsSorting) return;
            
            // Початок нового сортування
            ResetSortingState();
            IsSorting = true;
            _performanceTimer.Start();
            _timer?.Start();
        }

        private void PauseSorting()
        {
            if (!IsSorting) return;
            
            IsPaused = !IsPaused;
            
            if (IsPaused)
            {
                _timer?.Stop();
                _performanceTimer.Pause();
            }
            else
            {
                _timer?.Start();
                _performanceTimer.Resume();
            }
        }

        private void StepSorting()
        {
            if (!IsSorting)
            {
                // Якщо сортування ще не почалося, ініціалізуємо його
                ResetSortingState();
                IsSorting = true;
                _performanceTimer.Start();
            }
            else if (IsPaused)
            {
                // Якщо на паузі, відновлюємо таймер продуктивності
                _performanceTimer.Resume();
            }
            
            // Виконуємо один крок сортування
            PerformSortingStep();
            
            // Якщо не на паузі, ставимо на паузу
            if (!IsPaused)
            {
                IsPaused = true;
                _timer?.Stop();
                _performanceTimer.Pause();
            }
        }

        private void ResetSorting()
        {
            _timer?.Stop();
            ResetSortingState();
            DisplayArray();
        }

        private void ResetSortingState()
        {
            IsSorting = false;
            IsPaused = false;
            _i = 0;
            _j = 0;
            Comparisons = 0;
            Swaps = 0;
            ExecutionTime = "0 мс";
            _performanceTimer.Reset();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            PerformSortingStep();
        }

        private void PerformSortingStep()
        {
            if (_i >= _array.Length - 1)
            {
                // Сортування завершено
                FinishSorting();
                return;
            }

            // Оновлення часу виконання
            ExecutionTime = $"{_performanceTimer.ElapsedMilliseconds} мс";

            // Виконання кроку сортування бульбашкою
            if (_j < _array.Length - _i - 1)
            {
                // Порівняння сусідніх елементів
                Comparisons++;
                
                // Візуалізація порівняння (зміна кольору)
                UpdateBarColor(_j, "#FFFF00");     // Жовтий - поточний елемент
                UpdateBarColor(_j + 1, "#FFFF00"); // Жовтий - наступний елемент
                
                // Якщо поточний елемент більший за наступний, міняємо їх місцями
                if (_array[_j] > _array[_j + 1])
                {
                    Swaps++;
                    
                    // Обмін елементів
                    int temp = _array[_j];
                    _array[_j] = _array[_j + 1];
                    _array[_j + 1] = temp;
                    
                    // Візуалізація обміну (зміна висоти)
                    UpdateBarHeight(_j);
                    UpdateBarHeight(_j + 1);
                    
                    // Зміна кольору для обміну
                    UpdateBarColor(_j, "#FF0000");     // Червоний - елемент, який було переміщено
                    UpdateBarColor(_j + 1, "#00FF00"); // Зелений - елемент, на місце якого перемістили
                }
                
                _j++;
                
                // Відкладене відновлення кольорів
                Dispatcher.UIThread.Post(() =>
                {
                    // Повернення стандартного кольору для попередніх елементів
                    if (_j > 1) UpdateBarColor(_j - 2, "#0000FF"); // Синій - стандартний колір
                    if (_j > 0) UpdateBarColor(_j - 1, "#0000FF"); // Синій - стандартний колір
                }, DispatcherPriority.Background);
            }
            else
            {
                // Завершення внутрішнього циклу, перехід до наступного елемента зовнішнього циклу
                _j = 0;
                _i++;
                
                // Позначення відсортованого елемента
                if (_i > 0 && _array.Length - _i < Bars.Count)
                {
                    UpdateBarColor(_array.Length - _i, "#00FF00"); // Зелений - відсортований елемент
                }
            }
        }

        private void FinishSorting()
        {
            _timer?.Stop();
            IsSorting = false;
            IsPaused = false;
            _performanceTimer.Stop();
            ExecutionTime = $"{_performanceTimer.ElapsedMilliseconds} мс";
            
            // Позначення всіх елементів як відсортованих
            for (int i = 0; i < Bars.Count; i++)
            {
                UpdateBarColor(i, "#00FF00"); // Зелений - відсортований елемент
            }
        }

        private void UpdateBarHeight(int index)
        {
            if (index >= 0 && index < Bars.Count && index < _array.Length)
            {
                int maxValue = _array.Max();
                if (maxValue == 0) maxValue = 1; // Уникаємо ділення на нуль
                
                double barHeight = Math.Max((_array[index] / (double)maxValue) * _visualizationGrid.Bounds.Height * 0.9, 1);
                Bars[index].Height = barHeight;
            }
        }

        private void UpdateBarColor(int index, string colorHex)
        {
            if (index >= 0 && index < Bars.Count)
            {
                Bars[index].Background = new SolidColorBrush(Color.Parse(colorHex));
            }
        }

        public override void Dispose()
        {
            _timer?.Stop();
            _timer = null;
            base.Dispose();
        }
    }
}
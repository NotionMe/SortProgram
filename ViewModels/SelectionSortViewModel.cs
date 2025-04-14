using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Practika2_OPAM_Ubohyi_Stanislav.Algorithms;
using Practika2_OPAM_Ubohyi_Stanislav.Utils;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class SelectionSortViewModel : ViewModelBase
    {
        // Приватні поля
        private int[] _array = Array.Empty<int>();
        private DispatcherTimer? _timer;
        private int _i = 0, _j = 0, _minIndex = 0;
        private bool _isSorting = false;
        private bool _isPaused = false;
        private int _comparisons = 0;
        private int _swaps = 0;
        private PerformanceTimer _performanceTimer = new PerformanceTimer();
        private readonly Canvas _visualizationGrid;
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
        public SelectionSortViewModel(Canvas visualizationGrid)
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
                default: return 200; // За замовчуванням середньо
            }
        }

        private int GetArraySize()
        {
            // Перетворення індексу розміру масиву на кількість елементів
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
                default: return 20; // За замовчуванням 20 елементів
            }
        }

        private void GenerateArray()
        {
            // Зупиняємо сортування, якщо воно запущене
            if (_isSorting)
            {
                ResetSorting();
            }

            // Генеруємо новий масив відповідно до вибраного типу
            switch (_selectedArrayType)
            {
                case 0: // Випадковий
                    _array = ArrayGenerator.GenerateRandomArray(GetArraySize());
                    break;
                case 1: // Майже відсортований
                    _array = ArrayGenerator.GenerateNearlySortedArray(GetArraySize());
                    break;
                case 2: // Зворотній порядок
                    _array = ArrayGenerator.GenerateReversedArray(GetArraySize());
                    break;
                case 3: // Мало унікальних елементів
                    _array = ArrayGenerator.GenerateFewUniqueArray(GetArraySize());
                    break;
                default:
                    _array = ArrayGenerator.GenerateRandomArray(GetArraySize());
                    break;
            }

            // Відображаємо згенерований масив
            DisplayArray();
            
            // Скидаємо лічильники
            Comparisons = 0;
            Swaps = 0;
            ExecutionTime = "0 мс";
        }

        private void DisplayArray()
        {
            if (_array.Length == 0 || _visualizationGrid == null) return;

            // Очищаємо попередні елементи
            _visualizationGrid.Children.Clear();
            _bars.Clear();

            // Визначаємо розміри для відображення
            double gridWidth = _visualizationGrid.Bounds.Width;
            double gridHeight = _visualizationGrid.Bounds.Height;
            
            if (gridWidth <= 0 || gridHeight <= 0) return;

            // Знаходимо максимальне значення для масштабування
            int maxValue = 0;
            foreach (var value in _array)
            {
                if (value > maxValue) maxValue = value;
            }

            // Обчислюємо ширину стовпчика
            double barWidth = gridWidth / _array.Length;
            
            // Створюємо стовпчики для кожного елемента масиву
            for (int i = 0; i < _array.Length; i++)
            {
                // Обчислюємо висоту стовпчика пропорційно до значення
                double barHeight = (_array[i] / (double)maxValue) * gridHeight;
                
                // Створюємо стовпчик
                var bar = new Border
                {
                    Width = Math.Max(1, barWidth - 2), // Мінімальна ширина 1 піксель, з відступом 2 пікселі
                    Height = Math.Max(1, barHeight),   // Мінімальна висота 1 піксель
                    Background = new SolidColorBrush(Color.Parse("#0078D4")), // Синій колір
                    CornerRadius = new Avalonia.CornerRadius(2),
                    Margin = new Avalonia.Thickness(1)
                };
                
                // Додаємо стовпчик до колекції
                _bars.Add(bar);
                
                // Розміщуємо стовпчик у сітці
                Canvas.SetLeft(bar, i * barWidth);
                Canvas.SetBottom(bar, 0);
                
                // Додаємо стовпчик до сітки
                _visualizationGrid.Children.Add(bar);
            }
        }

        private void StartSorting()
        {
            if (_array.Length == 0) return;
            
            // Якщо сортування на паузі, відновлюємо його
            if (_isPaused)
            {
                _isPaused = false;
                _performanceTimer.Resume();
                _timer?.Start();
                return;
            }
            
            // Якщо сортування вже запущене, нічого не робимо
            if (_isSorting) return;
            
            // Ініціалізуємо змінні для алгоритму сортування вибором
            _i = 0;
            _j = 1;
            _minIndex = 0;
            _isSorting = true;
            
            // Скидаємо лічильники
            Comparisons = 0;
            Swaps = 0;
            
            // Запускаємо таймер продуктивності
            _performanceTimer.Start();
            
            // Запускаємо таймер для анімації
            _timer?.Start();
        }

        private void PauseSorting()
        {
            if (!_isSorting) return;
            
            _isPaused = true;
            _performanceTimer.Pause();
            _timer?.Stop();
        }

        private void StepSorting()
        {
            if (_array.Length == 0) return;
            
            // Якщо сортування не запущене, запускаємо його
            if (!_isSorting)
            {
                _i = 0;
                _j = 1;
                _minIndex = 0;
                _isSorting = true;
                
                // Скидаємо лічильники
                Comparisons = 0;
                Swaps = 0;
                
                // Запускаємо таймер продуктивності
                _performanceTimer.Start();
            }
            
            // Виконуємо один крок алгоритму
            PerformSortingStep();
        }

        private void ResetSorting()
        {
            // Зупиняємо таймер
            _timer?.Stop();
            
            // Скидаємо змінні
            _isSorting = false;
            _isPaused = false;
            _i = 0;
            _j = 1;
            _minIndex = 0;
            
            // Скидаємо таймер продуктивності
            _performanceTimer.Reset();
            
            // Відображаємо початковий масив
            DisplayArray();
            
            // Скидаємо лічильники
            Comparisons = 0;
            Swaps = 0;
            ExecutionTime = "0 мс";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Виконуємо один крок алгоритму при кожному тіку таймера
            PerformSortingStep();
        }

        private void PerformSortingStep()
        {
            if (_array.Length <= 1 || _i >= _array.Length - 1)
            {
                // Масив відсортований або порожній
                _timer?.Stop();
                _isSorting = false;
                _performanceTimer.Stop();
                ExecutionTime = $"{_performanceTimer.ElapsedMilliseconds} мс";
                return;
            }

            // Алгоритм сортування вибором
            if (_j == _i + 1)
            {
                // Початок нової ітерації зовнішнього циклу
                _minIndex = _i;
                UpdateBarColor(_minIndex, "#FF5722"); // Поточний мінімум (оранжевий)
                UpdateBarColor(_i, "#4CAF50");        // Поточна позиція (зелений)
            }

            if (_j < _array.Length)
            {
                // Порівнюємо поточний елемент з мінімальним
                Comparisons++;
                UpdateBarColor(_j, "#FFC107"); // Елемент, який порівнюємо (жовтий)

                if (_array[_j] < _array[_minIndex])
                {
                    // Знайдено новий мінімум
                    UpdateBarColor(_minIndex, "#0078D4"); // Повертаємо попередній мінімум до звичайного кольору
                    _minIndex = _j;
                    UpdateBarColor(_minIndex, "#FF5722"); // Новий мінімум (оранжевий)
                }
                else
                {
                    // Повертаємо звичайний колір для елемента, який не є мінімумом
                    UpdateBarColor(_j, "#0078D4");
                }

                _j++; // Переходимо до наступного елемента
            }
            else
            {
                // Завершення внутрішнього циклу, міняємо місцями елементи
                if (_minIndex != _i)
                {
                    // Обмін елементів
                    Swaps++;
                    int temp = _array[_i];
                    _array[_i] = _array[_minIndex];
                    _array[_minIndex] = temp;

                    // Оновлюємо відображення
                    UpdateBarHeight(_i, _array[_i]);
                    UpdateBarHeight(_minIndex, _array[_minIndex]);
                }

                // Повертаємо звичайний колір для всіх елементів
                UpdateBarColor(_i, "#0078D4");
                UpdateBarColor(_minIndex, "#0078D4");

                // Переходимо до наступної ітерації зовнішнього циклу
                _i++;
                _j = _i + 1;

                // Перевіряємо, чи завершено сортування
                if (_i >= _array.Length - 1)
                {
                    _timer?.Stop();
                    _isSorting = false;
                    _performanceTimer.Stop();
                    ExecutionTime = $"{_performanceTimer.ElapsedMilliseconds} мс";

                    // Виділяємо всі елементи як відсортовані
                    for (int k = 0; k < _array.Length; k++)
                    {
                        UpdateBarColor(k, "#4CAF50"); // Зелений колір для відсортованих елементів
                    }
                }
            }
        }

        private void UpdateBarColor(int index, string colorHex)
        {
            if (index < 0 || index >= _bars.Count) return;
            
            var bar = _bars[index];
            bar.Background = new SolidColorBrush(Color.Parse(colorHex));
        }

        private void UpdateBarHeight(int index, int value)
        {
            if (index < 0 || index >= _bars.Count || _visualizationGrid == null) return;
            
            var bar = _bars[index];
            
            // Знаходимо максимальне значення для масштабування
            int maxValue = 0;
            foreach (var val in _array)
            {
                if (val > maxValue) maxValue = val;
            }
            
            // Обчислюємо нову висоту стовпчика
            double gridHeight = _visualizationGrid.Bounds.Height;
            double barHeight = (value / (double)maxValue) * gridHeight;
            
            // Оновлюємо висоту стовпчика
            bar.Height = Math.Max(1, barHeight);
        }
    }
}

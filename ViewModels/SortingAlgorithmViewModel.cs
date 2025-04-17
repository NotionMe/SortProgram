using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Practika2_OPAM_Ubohyi_Stanislav.Utils;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public abstract class SortingAlgorithmViewModel : ViewModelBase, IDisposable
    {
        // Приватні поля
        protected int[] _array = Array.Empty<int>();
        protected DispatcherTimer? _timer;
        protected bool _isSorting = false;
        protected bool _isPaused = false;
        protected int _comparisons = 0;
        protected int _swaps = 0;
        protected PerformanceTimer _performanceTimer = new PerformanceTimer();
        protected int _selectedArraySize = 3; // Індекс 3 = 20 елементів
        protected int _selectedSpeed = 2; // Індекс 2 = Середня швидкість
        protected int _selectedArrayType = 0; // Індекс 0 = Випадковий масив
        protected string _executionTime = "0 мс";
        protected ObservableCollection<Border> _bars = new ObservableCollection<Border>();
        protected ISortingStrategy _sortingStrategy;

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
        public SortingAlgorithmViewModel(ISortingStrategy sortingStrategy)
        {
            _sortingStrategy = sortingStrategy;
            
            // Ініціалізація команд
            GenerateCommand = ReactiveCommand.Create(GenerateArray);
            StartCommand = ReactiveCommand.Create(StartSorting);
            PauseCommand = ReactiveCommand.Create(PauseSorting);
            StepCommand = ReactiveCommand.Create(StepSorting);
            ResetCommand = ReactiveCommand.Create(ResetSorting);
            
            // Ініціалізація таймера
            InitializeTimer();
            
            // Генерація початкового масиву
            _array = ArrayGenerator.GenerateRandomArray(GetArraySize());
        }

        // Методи
        protected void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GetSpeedInMilliseconds())
            };
            _timer.Tick += Timer_Tick;
        }

        protected int GetSpeedInMilliseconds()
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

        protected int GetArraySize()
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

        // Абстрактний метод для відображення масиву (різні контейнери в Avalonia)
        protected abstract void DisplayArray();
        
        // Абстрактний метод для оновлення висоти елементів
        protected abstract void UpdateBarHeight(int index);
        
        // Абстрактний метод для оновлення кольорів елементів
        protected abstract void UpdateBarColor(int index, string colorHex);

        public void StartSorting()
        {
            if (IsSorting && IsPaused)
            {
                // Відновлення сортування після паузи
                IsPaused = false;
                _performanceTimer.Resume();
                _timer?.Start();
                return;
            }
            
            if (IsSorting) return;
            
            // Початок нового сортування
            ResetSortingState();
            _sortingStrategy.Initialize(_array);
            IsSorting = true;
            _performanceTimer.Start();
            _timer?.Start();
        }

        public void PauseSorting()
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

        public void StepSorting()
        {
            if (!IsSorting)
            {
                // Якщо сортування ще не почалося, ініціалізуємо його
                ResetSortingState();
                _sortingStrategy.Initialize(_array);
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

        public void ResetSorting()
        {
            _timer?.Stop();
            ResetSortingState();
            DisplayArray();
        }

        protected void ResetSortingState()
        {
            IsSorting = false;
            IsPaused = false;
            Comparisons = 0;
            Swaps = 0;
            ExecutionTime = "0 мс";
            _performanceTimer.Reset();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            PerformSortingStep();
        }

        protected void PerformSortingStep()
        {
            // Оновлення часу виконання
            ExecutionTime = $"{_performanceTimer.ElapsedMilliseconds} мс";

            // Виконання кроку сортування через стратегію
            bool completed = _sortingStrategy.PerformStep(_array, ref _comparisons, ref _swaps);
            
            // Отримання індексів елементів для візуалізації
            (int index1, int index2, int minIndex) = _sortingStrategy.GetHighlightIndices();
            
            // Оновлення відображення на основі поточного стану
            UpdateVisualization(index1, index2, minIndex);
            
            // Перевірка чи сортування завершено
            if (completed)
            {
                FinishSorting();
            }
        }

        protected void UpdateVisualization(int index1, int index2, int minIndex = -1)
        {
            // Оновлення висоти всіх елементів
            for (int i = 0; i < _array.Length; i++)
            {
                UpdateBarHeight(i);
            }
            
            // Оновлення кольорів
            for (int i = 0; i < _bars.Count; i++)
            {
                if (i == index1)
                {
                    UpdateBarColor(i, "#FFFF00"); // Жовтий - поточний елемент
                }
                else if (i == index2)
                {
                    UpdateBarColor(i, "#FFFF00"); // Жовтий - наступний елемент
                }
                else if (i == minIndex && minIndex != -1)
                {
                    UpdateBarColor(i, "#FF5722"); // Оранжевий - мінімальний елемент (для Selection Sort)
                }
                else
                {
                    UpdateBarColor(i, "#0000FF"); // Синій - звичайний колір
                }
            }
        }

        protected void FinishSorting()
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

        public virtual void Dispose()
        {
            _timer?.Stop();
            _timer = null;
        }
    }
}
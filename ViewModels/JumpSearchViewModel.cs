using Avalonia.Media;
using Avalonia.Threading;
using Practika2_OPAM_Ubohyi_Stanislav.Utils;
using Practika2_OPAM_Ubohyi_Stanislav.Algorithms.Searching;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class JumpSearchViewModel : ViewModelBase
    {
        private int[] _internalArray = Array.Empty<int>();
        private DispatcherTimer? _timer;
        private bool _isSearching;
        private bool _isPaused;
        private int _currentPosition = -1;
        private int _jumpStep = 0;
        private int _valueToFind;
        private int _comparisons;
        private int _steps;
        private PerformanceTimer _performanceTimer = new PerformanceTimer();
        private Random _random = new Random();
        private bool _searchCompleted;
        private int _foundIndex = -1;
        private bool _isJumpPhase = true;

        private ObservableCollection<ArrayElementViewModel> _arrayElements = new ObservableCollection<ArrayElementViewModel>();
        public ObservableCollection<ArrayElementViewModel> ArrayElements
        {
            get => _arrayElements;
            set => this.RaiseAndSetIfChanged(ref _arrayElements, value);
        }

        private string _timeElapsed = "0 ms";
        public string TimeElapsed
        {
            get => _timeElapsed;
            set => this.RaiseAndSetIfChanged(ref _timeElapsed, value);
        }

        public int Comparisons
        {
            get => _comparisons;
            set => this.RaiseAndSetIfChanged(ref _comparisons, value);
        }

        public int Steps
        {
            get => _steps;
            set => this.RaiseAndSetIfChanged(ref _steps, value);
        }

        private string _searchStatusText = "?";
        public string SearchStatusText
        {
            get => _searchStatusText;
            set => this.RaiseAndSetIfChanged(ref _searchStatusText, value);
        }

        private IBrush _searchStatusColor = new SolidColorBrush(Colors.Gray);
        public IBrush SearchStatusColor
        {
            get => _searchStatusColor;
            set => this.RaiseAndSetIfChanged(ref _searchStatusColor, value);
        }

        public ReactiveCommand<Unit, Unit> StartCommand { get; }
        public ReactiveCommand<Unit, Unit> PauseCommand { get; }
        public ReactiveCommand<Unit, Unit> StepCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> GenerateCommand { get; }

        private int _selectedArraySizeIndex = 3; // Default to 20
        public int SelectedArraySizeIndex
        {
            get => _selectedArraySizeIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedArraySizeIndex, value);
        }

        private int _selectedArrayTypeIndex = 0; // Default to Random
        public int SelectedArrayTypeIndex
        {
            get => _selectedArrayTypeIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedArrayTypeIndex, value);
        }

        private int _selectedSpeedIndex = 2; // Default to Medium
        public int SelectedSpeedIndex
        {
            get => _selectedSpeedIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedSpeedIndex, value);
        }

        public JumpSearchViewModel()
        {
            InitializeTimer();
            SetupInitialArray();

            StartCommand = ReactiveCommand.Create(StartSearch);
            PauseCommand = ReactiveCommand.Create(PauseSearch);
            StepCommand = ReactiveCommand.Create(StepSearch);
            ResetCommand = ReactiveCommand.Create(ResetSearch);
            GenerateCommand = ReactiveCommand.Create(GenerateNewArray);
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GetSpeedInMilliseconds())
            };
            _timer.Tick += Timer_Tick;
        }

        private void UpdateTimerInterval()
        {
            if (_timer != null)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(GetSpeedInMilliseconds());
            }
        }

        private void SetupInitialArray()
        {
            ResetSearchStateInternal();
            CreateFreshArray();
            UpdateVisualization();
        }

        private void CreateFreshArray()
        {
            int size = GetArraySize();
            
            switch (SelectedArrayTypeIndex)
            {
                case 0: // Random
                    _internalArray = ArrayGenerator.GenerateRandomArray(size, 1, 100);
                    break;
                case 1: // Almost Sorted
                    _internalArray = ArrayGenerator.GenerateNearlySortedArray(size);
                    break;
                case 2: // Reverse Order
                    _internalArray = ArrayGenerator.GenerateReversedArray(size);
                    break;
                case 3: // Few Unique
                    _internalArray = ArrayGenerator.GenerateFewUniqueArray(size);
                    break;
                default:
                    _internalArray = ArrayGenerator.GenerateRandomArray(size, 1, 100);
                    break;
            }
            
            // Jump Search потребує відсортований масив
            Array.Sort(_internalArray);

            ArrayElements = new ObservableCollection<ArrayElementViewModel>(_internalArray.Select(x => new ArrayElementViewModel(x)));
        }

        private void GenerateNewArray()
        {
            SetupInitialArray();
        }

        private void StartSearch()
        {
            if (_isSearching && _isPaused)
            {
                _isPaused = false;
                _performanceTimer.Resume();
                _timer?.Start();
                UpdateSearchStatusText();
                return;
            }

            if (_isSearching) return;

            ResetSearchStateInternal();
            CreateFreshArray(); // Regenerate and sort array

            _isSearching = true;
            _currentPosition = -1; // Спеціальне значення для першого кроку
            _jumpStep = 0;
            _searchCompleted = false;
            _foundIndex = -1;
            _isJumpPhase = true;

            if (_internalArray.Length > 0)
            {
                _valueToFind = _internalArray[_random.Next(0, _internalArray.Length)];
            }
            else
            {
                _valueToFind = _random.Next(1, 101); // Fallback if array is empty
            }
            SearchStatusText = $"Шукаємо: {_valueToFind}";
            SearchStatusColor = new SolidColorBrush(Colors.Orange);

            UpdateTimerInterval();
            _performanceTimer.Start();
            _timer?.Start();
        }

        private void PauseSearch()
        {
            if (_isSearching && !_isPaused)
            {
                _isPaused = true;
                _timer?.Stop();
                _performanceTimer.Pause();
                SearchStatusText = "Пауза";
                SearchStatusColor = new SolidColorBrush(Colors.Gray);
            }
        }

        private void StepSearch()
        {
            if (!_isSearching || (_isSearching && !_isPaused && _timer != null && _timer.IsEnabled))
            {
                PauseSearch();
                _isPaused = true;
            }

            if (_searchCompleted) return;
            if (!_isSearching)
            {
                ResetSearchStateInternal();
                CreateFreshArray();
                _isSearching = true;
                _currentPosition = -1;
                _jumpStep = 0;
                _searchCompleted = false;
                _foundIndex = -1;
                _isJumpPhase = true;
                
                if (_internalArray.Length > 0) 
                    _valueToFind = _internalArray[_random.Next(0, _internalArray.Length)];
                else 
                    _valueToFind = _random.Next(1, 101);
                    
                _performanceTimer.Start(); // Start timer but don't start DispatcherTimer
            }

            PerformJumpSearchStep();
            UpdateVisualization();
            TimeElapsed = _performanceTimer.ElapsedMilliseconds.ToString("F2") + " ms";
        }

        private void ResetSearch()
        {
            ResetSearchStateInternal();
            CreateFreshArray(); // Create and sort a new array
            UpdateVisualization();
        }

        private void ResetSearchStateInternal()
        {
            _timer?.Stop();
            _isSearching = false;
            _isPaused = false;
            _searchCompleted = false;
            _currentPosition = -1;
            _jumpStep = 0;
            _isJumpPhase = true;
            _foundIndex = -1;
            Comparisons = 0;
            Steps = 0;
            _performanceTimer.Reset();
            TimeElapsed = "0 ms";
            SearchStatusText = "?";
            SearchStatusColor = new SolidColorBrush(Colors.Gray);

            foreach (var elem in ArrayElements)
            {
                elem.Background = new SolidColorBrush(Colors.LightGray);
                elem.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_isPaused || _searchCompleted)
            {
                _timer?.Stop();
                return;
            }

            PerformJumpSearchStep();
            UpdateVisualization();
            TimeElapsed = _performanceTimer.ElapsedMilliseconds.ToString("F2") + " ms";

            if (_searchCompleted)
            {
                _timer?.Stop();
                _performanceTimer.Stop();
            }
        }

        private void PerformJumpSearchStep()
        {
            Steps++;
            JumpSearchAlgorithmResult result = JumpSearchAlgorithm.SearchStep(_internalArray, _valueToFind, _currentPosition, _jumpStep, ref _comparisons);

            _currentPosition = result.CurrentPosition;
            _jumpStep = result.JumpStep;
            _searchCompleted = result.Completed;
            _isJumpPhase = result.IsJumpPhase;

            if (result.Completed)
            {
                if (result.Found)
                {
                    _foundIndex = result.CurrentPosition;
                    SearchStatusText = $"Знайдено {_valueToFind} на індексі {_foundIndex}!";
                    SearchStatusColor = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    _foundIndex = -1;
                    SearchStatusText = $"{_valueToFind} не знайдено.";
                    SearchStatusColor = new SolidColorBrush(Colors.Red);
                }
                _searchCompleted = true; // Явно встановлюємо завершення пошуку
                _timer?.Stop(); // Зупиняємо таймер
                _performanceTimer.Stop(); // Зупиняємо вимірювання часу
                HighlightCompletedState();
            }
            else
            {
                HighlightSearchArea(result.CurrentPosition, result.JumpStep, result.IsJumpPhase);
                UpdateSearchStatusText();
            }
        }

        private void UpdateSearchStatusText()
        {
            if (_isSearching && !_searchCompleted)
            {
                if (_isJumpPhase)
                {
                    SearchStatusText = $"Шукаємо: {_valueToFind} (Фаза стрибка: Позиція {_currentPosition}, Крок {_jumpStep})";
                }
                else
                {
                    SearchStatusText = $"Шукаємо: {_valueToFind} (Лінійний пошук: Позиція {_currentPosition})";
                }
                SearchStatusColor = new SolidColorBrush(Colors.Orange);
            }
        }

        private void HighlightSearchArea(int currentPosition, int jumpStep, bool isJumpPhase)
        {
            // Захист від неправильних значень
            if (currentPosition < 0 || currentPosition >= ArrayElements.Count)
            {
                return;
            }
            
            for (int i = 0; i < ArrayElements.Count; i++)
            {
                if (i == currentPosition)
                {
                    ArrayElements[i].Background = new SolidColorBrush(Colors.Orange); // Поточний елемент
                    ArrayElements[i].Foreground = new SolidColorBrush(Colors.White);
                }
                else if (isJumpPhase)
                {
                    // У фазі стрибка
                    if (i < currentPosition)
                    {
                        // Вже перевірені блоки
                        ArrayElements[i].Background = new SolidColorBrush(Colors.LightGray);
                        ArrayElements[i].Foreground = new SolidColorBrush(Colors.DarkGray);
                    }
                    else if (i == Math.Min(currentPosition + jumpStep, ArrayElements.Count - 1))
                    {
                        // Наступний блок, куди будемо стрибати
                        ArrayElements[i].Background = new SolidColorBrush(Color.FromArgb(255, 255, 200, 150)); // Світло-оранжевий
                        ArrayElements[i].Foreground = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        // Решта елементів
                        ArrayElements[i].Background = new SolidColorBrush(Colors.LightGray);
                        ArrayElements[i].Foreground = new SolidColorBrush(Colors.Black);
                    }
                }
                else
                {
                    // У фазі лінійного пошуку
                    int blockStart = Math.Max(0, currentPosition - (currentPosition % jumpStep));
                    int blockEnd = Math.Min(blockStart + jumpStep, ArrayElements.Count);
                    
                    if (i >= blockStart && i < blockEnd)
                    {
                        if (i < currentPosition)
                        {
                            // Елементи блоку, які вже перевірені в лінійному пошуку
                            ArrayElements[i].Background = new SolidColorBrush(Color.FromArgb(255, 173, 216, 230)); // LightBlue
                            ArrayElements[i].Foreground = new SolidColorBrush(Colors.Black);
                        }
                        else if (i > currentPosition)
                        {
                            // Елементи блоку, які ще не перевірені в лінійному пошуку
                            ArrayElements[i].Background = new SolidColorBrush(Color.FromArgb(255, 230, 230, 250)); // Lavender
                            ArrayElements[i].Foreground = new SolidColorBrush(Colors.Black);
                        }
                    }
                    else
                    {
                        // Елементи поза поточним блоком
                        ArrayElements[i].Background = new SolidColorBrush(Colors.LightGray);
                        ArrayElements[i].Foreground = new SolidColorBrush(Colors.DarkGray);
                    }
                }
            }
        }

        private void HighlightCompletedState()
        {
            for (int i = 0; i < ArrayElements.Count; i++)
            {
                if (i == _foundIndex) // Found
                {
                    ArrayElements[i].Background = new SolidColorBrush(Colors.Green);
                    ArrayElements[i].Foreground = new SolidColorBrush(Colors.White);
                }
                else // Not found or other elements
                {
                    ArrayElements[i].Background = new SolidColorBrush(Colors.LightGray);
                    ArrayElements[i].Foreground = new SolidColorBrush(Colors.DarkGray);
                }
            }
        }


        private void UpdateVisualization()
        {
            this.RaisePropertyChanged(nameof(ArrayElements));
        }

        private int GetArraySize()
        {
            return SelectedArraySizeIndex switch
            {
                0 => 5,
                1 => 10,
                2 => 15,
                3 => 20, // Default
                4 => 25,
                5 => 30,
                6 => 40,
                7 => 50,
                _ => 20
            };
        }

        private int GetSpeedInMilliseconds()
        {
            return SelectedSpeedIndex switch
            {
                0 => 2000, // Very Slow
                1 => 1000, // Slow
                2 => 500,  // Medium (Default)
                3 => 200,  // Fast
                4 => 50,   // Very Fast
                _ => 500
            };
        }
    }
}
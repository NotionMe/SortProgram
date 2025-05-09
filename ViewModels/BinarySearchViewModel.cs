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
    public class BinarySearchViewModel : ViewModelBase
    {
        private int[] _internalArray = Array.Empty<int>();
        private DispatcherTimer? _timer;
        private bool _isSearching;
        private bool _isPaused;
        private int _low;
        private int _high;
        private int _mid;
        private int _valueToFind;
        private int _comparisons;
        private int _steps;
        private PerformanceTimer _performanceTimer = new PerformanceTimer();
        private Random _random = new Random();
        private bool _searchCompleted;
        private int _foundIndex = -1;

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

        private bool _isArrowVisible;
        public bool IsArrowVisible
        {
            get => _isArrowVisible;
            set => this.RaiseAndSetIfChanged(ref _isArrowVisible, value);
        }

        private Avalonia.Thickness _arrowMargin;
        public Avalonia.Thickness ArrowMargin
        {
            get => _arrowMargin;
            set => this.RaiseAndSetIfChanged(ref _arrowMargin, value);
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

        public BinarySearchViewModel()
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
            _internalArray = ArrayGenerator.GenerateRandomArray(size, 1, 100);
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
            _low = 0;
            _high = _internalArray.Length - 1;
            _searchCompleted = false;
            _foundIndex = -1;

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
            IsArrowVisible = _internalArray.Length > 0;
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
                _low = 0;
                _high = _internalArray.Length - 1;
                _searchCompleted = false;
                _foundIndex = -1;
                if (_internalArray.Length > 0) _valueToFind = _internalArray[_random.Next(0, _internalArray.Length)];
                else _valueToFind = _random.Next(1, 101);
                _performanceTimer.Start(); // Start timer but don't start DispatcherTimer
                IsArrowVisible = _internalArray.Length > 0;
            }

            PerformBinarySearchStep();
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
            //_currentIndex = 0; // Not used in binary search like linear
            _low = 0;
            _high = 0;
            _mid = -1;
            _foundIndex = -1;
            Comparisons = 0;
            Steps = 0;
            _performanceTimer.Reset();
            TimeElapsed = "0 ms";
            SearchStatusText = "?";
            SearchStatusColor = new SolidColorBrush(Colors.Gray);
            IsArrowVisible = false;

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

            PerformBinarySearchStep();
            UpdateVisualization();
            TimeElapsed = _performanceTimer.ElapsedMilliseconds.ToString("F2") + " ms";

            if (_searchCompleted)
            {
                _timer?.Stop();
                _performanceTimer.Stop();
                IsArrowVisible = false; // Hide arrow once completed
            }
        }

        private void PerformBinarySearchStep()
        {
            Steps++;
            BinarySearchAlgorithmResult result = BinarySearchAlgorithm.SearchStep(_internalArray, _valueToFind, _low, _high, ref _comparisons);

            _mid = result.Mid;
            _searchCompleted = result.Completed;

            if (result.Completed)
            {
                if (result.Found)
                {
                    _foundIndex = result.Mid;
                    SearchStatusText = $"Знайдено {_valueToFind} на індексі {_foundIndex}!";
                    SearchStatusColor = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    _foundIndex = -1;
                    SearchStatusText = $"{_valueToFind} не знайдено.";
                    SearchStatusColor = new SolidColorBrush(Colors.Red);
                }
                HighlightCompletedState();
            }
            else
            {
                if (result.SearchInUpperHalf)
                {
                    // Element is in the right half
                    HighlightSearchArea(result.Mid + 1, _high, result.Mid, false);
                    _low = result.Mid + 1;
                }
                else
                {
                    // Element is in the left half
                    HighlightSearchArea(_low, result.Mid - 1, result.Mid, false);
                    _high = result.Mid - 1;
                }
                UpdateSearchStatusText();
            }
        }

        private void UpdateSearchStatusText()
        {
            if (_isSearching && !_searchCompleted)
            {
                SearchStatusText = $"Шукаємо: {_valueToFind} (Low: {_low}, High: {_high}, Mid: {_mid})";
                SearchStatusColor = new SolidColorBrush(Colors.Orange);
            }
        }

        private void HighlightSearchArea(int currentLow, int currentHigh, int currentMid, bool found)
        {
            for (int i = 0; i < ArrayElements.Count; i++)
            {
                if (found && i == currentMid)
                {
                    ArrayElements[i].Background = new SolidColorBrush(Colors.Green);
                    ArrayElements[i].Foreground = new SolidColorBrush(Colors.White);
                }
                else if (i == currentMid)
                {
                    ArrayElements[i].Background = new SolidColorBrush(Colors.Orange); // Current element being checked
                    ArrayElements[i].Foreground = new SolidColorBrush(Colors.White);
                }
                else if (i >= currentLow && i <= currentHigh)
                {
                    ArrayElements[i].Background = new SolidColorBrush(Color.FromArgb(255, 173, 216, 230)); // LightBlue for active search area
                    ArrayElements[i].Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    ArrayElements[i].Background = new SolidColorBrush(Colors.LightGray); // Elements outside current search range
                    ArrayElements[i].Foreground = new SolidColorBrush(Colors.DarkGray);
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

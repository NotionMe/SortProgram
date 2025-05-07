using Avalonia.Media;
using Avalonia.Threading;
using Practika2_OPAM_Ubohyi_Stanislav.Algorithms.Searching;
using Practika2_OPAM_Ubohyi_Stanislav.Utils;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks; 

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class LinearSearchViewModel : ViewModelBase
    {
        private int[] _internalArray = Array.Empty<int>();
        private DispatcherTimer? _timer;
        private bool _isSearching;
        private bool _isPaused;
        private int _currentIndex;
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
        
        private int _selectedArraySizeIndex = 3;
        public int SelectedArraySizeIndex
        {
            get => _selectedArraySizeIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedArraySizeIndex, value);
        }

        private int _selectedArrayTypeIndex = 0;
        public int SelectedArrayTypeIndex
        {
            get => _selectedArrayTypeIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedArrayTypeIndex, value);
        }

        private int _selectedSpeedIndex = 2;
        public int SelectedSpeedIndex
        {
            get => _selectedSpeedIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedSpeedIndex, value);
        }


        public LinearSearchViewModel()
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
            int arrayType = SelectedArrayTypeIndex;

            switch (arrayType)
            {
                case 0: _internalArray = ArrayGenerator.GenerateRandomArray(size); break;
                case 1: _internalArray = ArrayGenerator.GenerateNearlySortedArray(size); break;
                case 2: _internalArray = ArrayGenerator.GenerateReversedArray(size); break;
                case 3: _internalArray = ArrayGenerator.GenerateFewUniqueArray(size); break;
                default: _internalArray = ArrayGenerator.GenerateRandomArray(size); break;
            }
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
            _isSearching = true;
            _valueToFind = _random.Next(1, 101);

            CreateFreshArray();

            if (_internalArray.Length > 0)
            {
                if (!_internalArray.Contains(_valueToFind))
                {
                    int randomIndexToReplace = _random.Next(0, _internalArray.Length);
                    _internalArray[randomIndexToReplace] = _valueToFind;
                    if (ArrayElements.Count > randomIndexToReplace)
                    {
                        ArrayElements[randomIndexToReplace].Value = _valueToFind;
                    }
                }
            }
            
            UpdateTimerInterval();
            _performanceTimer.Start();
            _timer?.Start();
            UpdateVisualization();
        }

        private void PauseSearch()
        {
            if (!_isSearching) return;

            _isPaused = !_isPaused;

            if (_isPaused)
            {
                _timer?.Stop();
                _performanceTimer.Pause();
            }
            else
            {
                _timer?.Start();
                _performanceTimer.Resume();
            }
            UpdateSearchStatusText();
        }

        private void StepSearch()
        {
             UpdateTimerInterval(); 
            if (!_isSearching)
            {
                ResetSearchStateInternal();
                _isSearching = true;
                _valueToFind = _random.Next(1, 101);
                CreateFreshArray();
                if (_internalArray.Length > 0 && !_internalArray.Contains(_valueToFind))
                {
                    int randomIndexToReplace = _random.Next(0, _internalArray.Length);
                    _internalArray[randomIndexToReplace] = _valueToFind;
                     if (ArrayElements.Count > randomIndexToReplace)
                    {
                        ArrayElements[randomIndexToReplace].Value = _valueToFind;
                    }
                }
                _performanceTimer.Start();
            }
            else if (_isPaused)
            {
                _performanceTimer.Resume();
            }

            PerformSearchStep(); 

            if (_isSearching && !_searchCompleted)
            {
                if (!_isPaused) 
                {
                    _isPaused = true;
                    _timer?.Stop();
                    _performanceTimer.Pause();
                }
            }
            UpdateSearchStatusText(); 
        }

        private void ResetSearch()
        {
            _timer?.Stop();
            SetupInitialArray();
        }
        
        private void UpdateVisualization()
        {
            TimeElapsed = $"{_performanceTimer.ElapsedMilliseconds} ms";
            Comparisons = _comparisons; 
            Steps = _steps;             

            for (int i = 0; i < ArrayElements.Count; i++)
            {
                ArrayElements[i].Background = GetElementBrush(i);
                ArrayElements[i].Foreground = GetElementTextBrush(i);
            }
            
            UpdateSearchStatusText();
        }
        
        


        private IBrush GetElementBrush(int index)
        {
            if (!_isSearching && !_searchCompleted) return new SolidColorBrush(Colors.SlateGray);
            if (_searchCompleted && index == _foundIndex) return new SolidColorBrush(Colors.Green);
            if (index < _currentIndex && _isSearching) return new SolidColorBrush(Colors.DimGray); 
            if (index == _currentIndex && _isSearching && !_searchCompleted) return new SolidColorBrush(Colors.Orange); 
            return new SolidColorBrush(Colors.CornflowerBlue);
        }

        private IBrush GetElementTextBrush(int index)
        {
            return new SolidColorBrush(Colors.White);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            PerformSearchStep();
        }

        private void PerformSearchStep()
        {
            TimeElapsed = $"{_performanceTimer.ElapsedMilliseconds} ms";

            if (_currentIndex >= _internalArray.Length)
            {
                FinishSearching(false); 
                return;
            }

            Comparisons++;
            Steps++;

            if (_internalArray[_currentIndex] == _valueToFind)
            {
                _foundIndex = _currentIndex;
                FinishSearching(true); 
                return;
            }

            _currentIndex++;
            UpdateVisualization(); // Arrow position is updated as part of visualization
        }

        private void FinishSearching(bool found)
        {
            _timer?.Stop();
            _isSearching = false;
            _searchCompleted = true;
            _performanceTimer.Stop();
            
            UpdateVisualization();
        }
        
        private void UpdateSearchStatusText()
        {
            if (!_isSearching && !_searchCompleted)
            {
                SearchStatusText = "?";
                SearchStatusColor = new SolidColorBrush(Colors.Gray);
            }
            else if (_searchCompleted)
            {
                if (_foundIndex != -1)
                {
                    SearchStatusText = $"Found at index {_foundIndex}";
                    SearchStatusColor = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    SearchStatusText = "Not found";
                    SearchStatusColor = new SolidColorBrush(Colors.Red);
                }
            }
            else if (_isSearching && _isPaused)
            {
                 SearchStatusText = $"Paused at index {_currentIndex}... Searching for {_valueToFind}";
                 SearchStatusColor = new SolidColorBrush(Colors.DarkOrange);
            }
            else if (_isSearching)
            {
                SearchStatusText = $"Searching for {_valueToFind}... (Checking index {_currentIndex})";
                SearchStatusColor = new SolidColorBrush(Colors.Orange);
            }
        }


        private void ResetSearchStateInternal()
        {
            _isSearching = false;
            _isPaused = false;
            _currentIndex = 0;
            Comparisons = 0;
            Steps = 0;
            _searchCompleted = false;
            _foundIndex = -1;
            _performanceTimer.Reset();
            TimeElapsed = "0 ms";
            IsArrowVisible = false;
            UpdateSearchStatusText();
        }

        private int GetArraySize()
        {
            switch (SelectedArraySizeIndex)
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

        private int GetSpeedInMilliseconds()
        {
            switch (SelectedSpeedIndex)
            {
                case 0: return 1000; 
                case 1: return 500;  
                case 2: return 200;  
                case 3: return 50;   
                case 4: return 10;   
                default: return 200;
            }
        }
    }
}

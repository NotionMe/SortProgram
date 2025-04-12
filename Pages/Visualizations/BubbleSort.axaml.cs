using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Practika2_OPAM_Ubohyi_Stanislav.Algorithms;
using Practika2_OPAM_Ubohyi_Stanislav.Utils;

namespace Practika2_OPAM_Ubohyi_Stanislav.Pages.Visualizations
{
    public partial class BubbleSort : UserControl
    {
        private int[] array = Array.Empty<int>();
        private List<Border> bars = new List<Border>();
        private DispatcherTimer? timer;
        private int i = 0, j = 0;
        private bool isSorting = false;
        private bool isPaused = false;
        private int comparisons = 0;
        private int swaps = 0;
        private PerformanceTimer performanceTimer = new PerformanceTimer();
        private bool isInitialized = false;
        private bool isLayoutComplete = false;
        private const int DEFAULT_ARRAY_SIZE = 20;
        private const int DEFAULT_SPEED = 5;

        public BubbleSort()
        {
            InitializeComponent();
            InitializeTimer();
            
            // Створюємо масив за замовчуванням для уникнення NullReferenceException
            array = ArrayGenerator.GenerateRandomArray(DEFAULT_ARRAY_SIZE);
            
            // Відкладаємо ініціалізацію GUI елементів
            this.AttachedToVisualTree += (s, e) =>
            {
                isInitialized = true;
                
                // Відкладаємо ініціалізацію до наступного циклу оновлення UI
                Dispatcher.UIThread.Post(() =>
                {
                    isLayoutComplete = true;
                    GenerateArray();
                }, DispatcherPriority.Loaded);
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            timer.Tick += Timer_Tick!;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (isSorting && !isPaused)
            {
                _ = PerformSortStep();
            }
        }

        private void GenerateArray()
        {
            // Перевіряємо, чи завантажено UI
            if (!isInitialized || !isLayoutComplete) return;
            
            try
            {
                // Безпечно отримуємо елементи управління
                var sizeSlider = this.FindControl<Slider>("ArraySizeSlider");
                var typeComboBox = this.FindControl<ComboBox>("ArrayTypeComboBox");
                
                // Якщо елементи не знайдені, використовуємо значення за замовчуванням
                int size = sizeSlider != null ? (int)sizeSlider.Value : DEFAULT_ARRAY_SIZE;
                string? arrayType = (typeComboBox?.SelectedItem as ComboBoxItem)?.Content?.ToString();

                // Якщо не вдалося отримати тип масиву, використовуємо випадковий
                if (string.IsNullOrEmpty(arrayType))
                {
                    array = ArrayGenerator.GenerateRandomArray(size);
                }
                else
                {
                    array = arrayType switch
                    {
                        "Майже відсортований" => ArrayGenerator.GenerateNearlySortedArray(size),
                        "Зворотній порядок" => ArrayGenerator.GenerateReversedArray(size),
                        "Мало унікальних елементів" => ArrayGenerator.GenerateFewUniqueArray(size),
                        _ => ArrayGenerator.GenerateRandomArray(size)
                    };
                }

                VisualizeArray();
                ResetCounters();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при генерації масиву: {ex.Message}");
                // Використовуємо масив за замовчуванням у випадку помилки
                array = ArrayGenerator.GenerateRandomArray(DEFAULT_ARRAY_SIZE);
                VisualizeArray();
                ResetCounters();
            }
        }

        private void VisualizeArray()
        {
            // Перевіряємо, чи завантажено UI і чи є доступ до потрібних елементів
            if (!isInitialized || !isLayoutComplete) return;
            
            try
            {
                var visualizationGrid = this.FindControl<Grid>("VisualizationGrid");
                if (visualizationGrid == null) return;
                
                visualizationGrid.Children.Clear();
                bars.Clear();

                // Перевіряємо, чи масив не порожній
                if (array == null || array.Length == 0) return;

                int max = array.Max();
                
                // Перевіряємо розмір контейнера
                if (visualizationGrid.Bounds.Width <= 0 || visualizationGrid.Bounds.Height <= 0)
                {
                    // Якщо розміри ще не встановлені, відкладаємо візуалізацію
                    Dispatcher.UIThread.Post(() => VisualizeArray(), DispatcherPriority.Render);
                    return;
                }
                
                double width = visualizationGrid.Bounds.Width / array.Length;
                double heightFactor = visualizationGrid.Bounds.Height / max;

                for (int i = 0; i < array.Length; i++)
                {
                    double height = array[i] * heightFactor;
                    
                    var bar = new Border
                    {
                        Width = Math.Max(width - 2, 1),
                        Height = height,
                        Background = new SolidColorBrush(Colors.Blue),
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                        Margin = new Thickness(1),
                        [Grid.ColumnProperty] = i
                    };
                    
                    bars.Add(bar);
                    visualizationGrid.Children.Add(bar);
                }

                // Додаємо стовпці для кожного елемента масиву
                visualizationGrid.ColumnDefinitions.Clear();
                for (int i = 0; i < array.Length; i++)
                {
                    visualizationGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Parse("*")));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при візуалізації масиву: {ex.Message}");
            }
        }

        private void UpdateVisualization()
        {
            // Перевіряємо, чи масив і візуальні елементи вже створені
            if (array == null || bars == null || bars.Count == 0 || bars.Count != array.Length) return;
            
            try
            {
                for (int k = 0; k < array.Length; k++)
                {
                    bars[k].Background = new SolidColorBrush(Colors.Blue);
                    if (k < array.Length - i)
                    {
                        if (k == j)
                        {
                            bars[k].Background = new SolidColorBrush(Colors.Red); // Поточний елемент
                        }
                        if (k == j + 1 && j + 1 < array.Length)
                        {
                            bars[k].Background = new SolidColorBrush(Colors.Green); // Наступний елемент
                        }
                    }
                    else
                    {
                        bars[k].Background = new SolidColorBrush(Colors.Purple); // Відсортована частина
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при оновленні візуалізації: {ex.Message}");
            }
        }

        private async Task PerformSortStep()
        {
            // Перевіряємо наявність масиву та елементів інтерфейсу
            if (array == null || array.Length <= 1 || bars == null || bars.Count == 0) return;
            
            try
            {
                var comparisonsTextBlock = this.FindControl<TextBlock>("ComparisonsTextBlock");
                var swapsTextBlock = this.FindControl<TextBlock>("SwapsTextBlock");
                var visualizationGrid = this.FindControl<Grid>("VisualizationGrid");
                
                if (i >= array.Length - 1 || j >= array.Length - i - 1)
                {
                    if (i >= array.Length - 1)
                    {
                        // Сортування завершено
                        SortingCompleted();
                        return;
                    }
                    j = 0;
                    i++;
                }

                // Порівнюємо сусідні елементи, якщо вони в межах масиву
                if (j + 1 < array.Length)
                {
                    comparisons++;
                    if (comparisonsTextBlock != null)
                    {
                        comparisonsTextBlock.Text = comparisons.ToString();
                    }

                    if (array[j] > array[j + 1])
                    {
                        // Міняємо елементи місцями
                        swaps++;
                        if (swapsTextBlock != null)
                        {
                            swapsTextBlock.Text = swaps.ToString();
                        }

                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;

                        // Оновлюємо візуалізацію після обміну
                        if (visualizationGrid != null && visualizationGrid.Bounds.Height > 0 && bars.Count > j + 1)
                        {
                            double heightFactor = visualizationGrid.Bounds.Height / array.Max();
                            bars[j].Height = array[j] * heightFactor;
                            bars[j + 1].Height = array[j + 1] * heightFactor;
                        }
                    }
                }

                UpdateVisualization();
                j++;
                
                // Додаємо затримку для плавної анімації
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при виконанні кроку сортування: {ex.Message}");
            }
        }

        private void SortingCompleted()
        {
            try
            {
                // Перевіряємо наявність необхідних елементів
                var timeTextBlock = this.FindControl<TextBlock>("TimeTextBlock");
                
                isSorting = false;
                if (timer != null)
                {
                    timer.Stop();
                }
                performanceTimer.Stop();
                
                if (timeTextBlock != null)
                {
                    timeTextBlock.Text = $"{performanceTimer.ElapsedMilliseconds} мс";
                }

                // Позначаємо всі елементи як відсортовані, якщо вони існують
                if (bars != null)
                {
                    foreach (var bar in bars)
                    {
                        bar.Background = new SolidColorBrush(Colors.Purple);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завершенні сортування: {ex.Message}");
            }
        }

        private void ResetCounters()
        {
            try
            {
                // Перевіряємо наявність елементів інтерфейсу
                var comparisonsTextBlock = this.FindControl<TextBlock>("ComparisonsTextBlock");
                var swapsTextBlock = this.FindControl<TextBlock>("SwapsTextBlock");
                var timeTextBlock = this.FindControl<TextBlock>("TimeTextBlock");
                
                i = 0;
                j = 0;
                comparisons = 0;
                swaps = 0;
                
                if (comparisonsTextBlock != null) comparisonsTextBlock.Text = "0";
                if (swapsTextBlock != null) swapsTextBlock.Text = "0";
                if (timeTextBlock != null) timeTextBlock.Text = "0 мс";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при скиданні лічильників: {ex.Message}");
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSorting)
            {
                isSorting = false;
                isPaused = false;
                if (timer != null)
                {
                    timer.Stop();
                }
            }
            GenerateArray();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (timer == null) InitializeTimer();
            
            if (!isSorting)
            {
                try
                {
                    isSorting = true;
                    isPaused = false;
                    
                    // Безпечно отримуємо елемент управління
                    var speedSlider = this.FindControl<Slider>("SpeedSlider");
                    
                    // Якщо елемент не знайдений, використовуємо значення за замовчуванням
                    double speed = speedSlider != null ? speedSlider.Value : DEFAULT_SPEED;
                    
                    // Встановлюємо інтервал таймера
                    if (timer != null)
                    {
                        timer.Interval = TimeSpan.FromMilliseconds(1000 / speed);
                        timer.Start();
                    }
                    
                    performanceTimer.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при запуску сортування: {ex.Message}");
                    isSorting = false;
                }
            }
            else if (isPaused)
            {
                isPaused = false;
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSorting && !isPaused)
            {
                isPaused = true;
            }
        }

        private async void StepButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isSorting)
                {
                    isSorting = true;
                    isPaused = true;
                    performanceTimer.Start();
                }
                
                if (isPaused)
                {
                    await PerformSortStep();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при виконанні кроку: {ex.Message}");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isSorting)
                {
                    isSorting = false;
                    isPaused = false;
                    if (timer != null)
                    {
                        timer.Stop();
                    }
                }

                GenerateArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при скиданні: {ex.Message}");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isSorting)
                {
                    if (timer != null)
                    {
                        timer.Stop();
                    }
                    isSorting = false;
                }

                // Спочатку спробуємо отримати доступ до головного вікна
                var mainWindow = this.VisualRoot as Practika2_OPAM_Ubohyi_Stanislav.SortProgram;
                if (mainWindow != null)
                {
                    // Якщо головне вікно доступне, використовуємо його метод навігації
                    mainWindow.NavigateToPagePublic(new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage());
                }
                else if (this.Parent is ContentControl contentControl)
                {
                    // Альтернативний метод навігації
                    contentControl.Content = new Practika2_OPAM_Ubohyi_Stanislav.Pages.SortingAlgorithmsPage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при поверненні: {ex.Message}");
            }
        }
    }
} 
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Practika2_OPAM_Ubohyi_Stanislav.Algorithms;
using System;
using System.Linq;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class QuickSortViewModel : SortingAlgorithmViewModel
    {
        private readonly Grid _visualizationGrid;

        public QuickSortViewModel(Grid visualizationGrid) 
            : base(new QuickSortStrategy())
        {
            _visualizationGrid = visualizationGrid;
            
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

        protected override void DisplayArray()
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
                    Background = new SolidColorBrush(Color.Parse("#00FF00")), // Зелений колір
                    Margin = new Thickness(spacing/2),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                    CornerRadius = new CornerRadius(6, 6, 0, 0), // Додано закруглення верхніх кутів
                    BoxShadow = new BoxShadows(new BoxShadow { Blur = 4, OffsetX = 0, OffsetY = 2, Spread = 0, Color = new Color(0, 0, 0, (byte)0.2) }) // Додано тінь
                };
                
                // Встановлюємо колонку для стовпчика
                Grid.SetColumn(bar, i);
                
                Bars.Add(bar);
                _visualizationGrid.Children.Add(bar);
            }
        }

        protected override void UpdateBarHeight(int index)
        {
            if (index >= 0 && index < Bars.Count && index < _array.Length)
            {
                int maxValue = _array.Max();
                if (maxValue == 0) maxValue = 1; // Уникаємо ділення на нуль
                
                double barHeight = Math.Max((_array[index] / (double)maxValue) * _visualizationGrid.Bounds.Height * 0.9, 1);
                Bars[index].Height = barHeight;
            }
        }

        protected override void UpdateBarColor(int index, string colorHex)
        {
            if (index >= 0 && index < Bars.Count)
            {
                Bars[index].Background = new SolidColorBrush(Color.Parse(colorHex));
            }
        }
    }
}
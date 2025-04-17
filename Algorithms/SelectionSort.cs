using System;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms
{
    public class SelectionSort
    {
        public static int[] Sort(int[] array)
        {
            int n = array.Length;
            int[] result = (int[])array.Clone(); 
            
            for (int i = 0; i < n - 1; i++)
            {
                // Знаходимо мінімальний елемент у невідсортованій частині
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (result[j] < result[minIndex])
                    {
                        minIndex = j;
                    }
                }
                
                // Міняємо знайдений мінімальний елемент з першим елементом
                if (minIndex != i)
                {
                    int temp = result[i];
                    result[i] = result[minIndex];
                    result[minIndex] = temp;
                }
            }
            
            return result;
        }
    }

    public class SelectionSortStrategy : ISortingStrategy
    {
        private int _i = 0, _j = 0, _minIndex = 0;

        public void Initialize(int[] array)
        {
            _i = 0;
            _j = 1; // Починаємо з 1-го елемента (після i)
            _minIndex = 0;
        }

        public bool PerformStep(int[] array, ref int comparisons, ref int swaps)
        {
            if (_i >= array.Length - 1)
            {
                // Сортування завершено
                return true;
            }

            // Алгоритм сортування вибором
            if (_j == _i + 1)
            {
                // Початок нової ітерації зовнішнього циклу
                _minIndex = _i;
            }

            if (_j < array.Length)
            {
                // Порівнюємо поточний елемент з мінімальним
                comparisons++;

                if (array[_j] < array[_minIndex])
                {
                    // Знайдено новий мінімум
                    _minIndex = _j;
                }

                _j++; // Переходимо до наступного елемента
            }
            else
            {
                // Завершення внутрішнього циклу, міняємо місцями елементи
                if (_minIndex != _i)
                {
                    // Обмін елементів
                    swaps++;
                    int temp = array[_i];
                    array[_i] = array[_minIndex];
                    array[_minIndex] = temp;
                }

                // Переходимо до наступної ітерації зовнішнього циклу
                _i++;
                _j = _i + 1;
                
                // Перевіряємо, чи завершено сортування
                if (_i >= array.Length - 1)
                {
                    return true;
                }
            }

            return false;
        }

        public (int, int, int) GetHighlightIndices()
        {
            return (_i, _j, _minIndex); // Поточний індекс, індекс порівняння, індекс мінімуму
        }
    }
}
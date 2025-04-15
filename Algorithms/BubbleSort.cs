using System;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms
{
    public class BubbleSort
    {
        public static int[] Sort(int[] array)
        {
            int n = array.Length;
            int[] result = (int[])array.Clone(); // Копіюємо масив, щоб не змінювати оригінал
            
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (result[j] > result[j + 1])
                    {
                        // Міняємо елементи місцями
                        int temp = result[j];
                        result[j] = result[j + 1];
                        result[j + 1] = temp;
                    }
                }
            }
            
            return result;
        }
    }

    public class BubbleSortStrategy : ISortingStrategy
    {
        private int _i = 0, _j = 0;

        public void Initialize(int[] array)
        {
            _i = 0;
            _j = 0;
        }

        public bool PerformStep(int[] array, ref int comparisons, ref int swaps)
        {
            if (_i >= array.Length - 1)
            {
                // Сортування завершено
                return true;
            }

            // Виконання кроку сортування бульбашкою
            if (_j < array.Length - _i - 1)
            {
                // Порівняння сусідніх елементів
                comparisons++;
                
                // Якщо поточний елемент більший за наступний, міняємо їх місцями
                if (array[_j] > array[_j + 1])
                {
                    swaps++;
                    
                    // Обмін елементів
                    int temp = array[_j];
                    array[_j] = array[_j + 1];
                    array[_j + 1] = temp;
                }
                
                _j++;
            }
            else
            {
                // Завершення внутрішнього циклу, перехід до наступного елемента зовнішнього циклу
                _j = 0;
                _i++;
            }

            return false;
        }

        public (int, int, int) GetHighlightIndices()
        {
            return (_j, _j + 1, -1); // Перший індекс, другий індекс, індекс мінімуму (не використовується в Bubble Sort)
        }
    }
}
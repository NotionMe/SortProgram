using System;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms
{
    /// <summary>
    /// Реалізація алгоритму сортування бульбашкою
    /// </summary>
    public class BubbleSort
    {
        /// <summary>
        /// Виконує сортування масиву методом бульбашки
        /// </summary>
        /// <param name="array">Вхідний масив для сортування</param>
        /// <returns>Відсортований масив</returns>
        public static int[] Sort(int[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
                
            int n = array.Length;
            int[] result = (int[])array.Clone();
            
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (result[j] > result[j + 1])
                    {
                        Swap(ref result[j], ref result[j + 1]);
                    }
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Допоміжний метод для обміну значень двох елементів
        /// </summary>
        private static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }

    /// <summary>
    /// Стратегія покрокового виконання алгоритму сортування бульбашкою для візуалізації
    /// </summary>
    public class BubbleSortStrategy : ISortingStrategy
    {
        private int _currentIteration = 0;
        private int _currentPosition = 0;

        /// <summary>
        /// Ініціалізує стан стратегії для початку сортування
        /// </summary>
        public void Initialize(int[] array)
        {
            _currentIteration = 0;
            _currentPosition = 0;
        }

        /// <summary>
        /// Виконує один крок алгоритму сортування бульбашкою
        /// </summary>
        /// <returns>True якщо сортування завершено, інакше False</returns>
        public bool PerformStep(int[] array, ref int comparisons, ref int swaps)
        {
            // Перевірка чи сортування завершено
            if (_currentIteration >= array.Length - 1)
            {
                return true;
            }

            // Виконання кроку алгоритму
            if (_currentPosition < array.Length - _currentIteration - 1)
            {
                comparisons++;
                
                // Порівнюємо поточний елемент з наступним
                if (array[_currentPosition] > array[_currentPosition + 1])
                {
                    swaps++;
                    
                    // Обмін елементів
                    int temp = array[_currentPosition];
                    array[_currentPosition] = array[_currentPosition + 1];
                    array[_currentPosition + 1] = temp;
                }
                
                _currentPosition++;
            }
            else
            {
                // Перехід до наступної ітерації зовнішнього циклу
                _currentPosition = 0;
                _currentIteration++;
            }

            return false;
        }

        /// <summary>
        /// Повертає індекси елементів для візуального виділення
        /// </summary>
        /// <returns>Кортеж з індексами (поточний, наступний, додатковий)</returns>
        public (int, int, int) GetHighlightIndices()
        {
            return (_currentPosition, _currentPosition + 1, -1);
        }
    }
}
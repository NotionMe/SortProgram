using System;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms.Searching
{
    /// <summary>
    /// Реалізація алгоритму лінійного пошуку
    /// </summary>
    public class LinearSearch
    {
        /// <summary>
        /// Виконує лінійний пошук елемента в масиві
        /// </summary>
        /// <param name="array">Масив для пошуку</param>
        /// <param name="value">Значення для пошуку</param>
        /// <returns>Індекс знайденого елемента або -1, якщо елемент не знайдено</returns>
        public static int Search(int[] array, int value)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
                
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    return i; // Знайдено елемент, повертаємо його індекс
                }
            }
            
            return -1; // Елемент не знайдено
        }
    }

    /// <summary>
    /// Стратегія покрокового виконання алгоритму лінійного пошуку для візуалізації
    /// </summary>
    public class LinearSearchStrategy : ISearchingStrategy
    {
        private int _currentIndex = 0;
        private int _foundIndex = -1;
        private bool _searchCompleted = false;
        private int _valueToFind;

        /// <summary>
        /// Ініціалізує стан стратегії для початку пошуку
        /// </summary>
        public void Initialize(int[] array, int valueToFind)
        {
            _currentIndex = 0;
            _foundIndex = -1;
            _searchCompleted = false;
            _valueToFind = valueToFind;
        }

        /// <summary>
        /// Виконує один крок алгоритму лінійного пошуку
        /// </summary>
        /// <returns>True якщо пошук завершено, інакше False</returns>
        public bool PerformStep(int[] array, ref int comparisons)
        {
            // Перевірка чи пошук завершено
            if (_searchCompleted || _currentIndex >= array.Length)
            {
                _searchCompleted = true;
                return true;
            }

            // Збільшення лічильника порівнянь
            comparisons++;

            // Перевірка поточного елемента
            if (array[_currentIndex] == _valueToFind)
            {
                _foundIndex = _currentIndex;
                _searchCompleted = true;
                return true;
            }

            // Перехід до наступного елемента
            _currentIndex++;

            // Перевірка чи досягнуто кінця масиву
            if (_currentIndex >= array.Length)
            {
                _searchCompleted = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Повертає індекси елементів для візуального виділення
        /// </summary>
        /// <returns>Кортеж з індексами (поточний, індекс знайденого елемента, -1)</returns>
        public (int, int, int) GetHighlightIndices()
        {
            return (_currentIndex, _foundIndex, -1);
        }

        /// <summary>
        /// Повертає знайдений індекс або -1, якщо елемент не знайдено
        /// </summary>
        public int GetFoundIndex()
        {
            return _foundIndex;
        }

        /// <summary>
        /// Повертає значення, яке шукаємо
        /// </summary>
        public int GetValueToFind()
        {
            return _valueToFind;
        }
    }
}
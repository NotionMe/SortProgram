using System;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms
{
    public class InsertionSort
    {
        public static int[] Sort(int[] array)
        {
            int n = array.Length;
            int[] result = (int[])array.Clone(); // Копіюємо масив, щоб не змінювати оригінал
            
            for (int i = 1; i < n; i++)
            {
                int key = result[i];
                int j = i - 1;
                
                // Переміщуємо елементи, більші за key, на одну позицію вперед
                while (j >= 0 && result[j] > key)
                {
                    result[j + 1] = result[j];
                    j = j - 1;
                }
                
                result[j + 1] = key;
            }
            
            return result;
        }
    }

    public class InsertionSortStrategy : ISortingStrategy
    {
        private int _i = 1;
        private int _j;
        private int _key;
        private bool _innerLoopStarted = false;
        private bool _keyInserted = true;

        public void Initialize(int[] array)
        {
            _i = 1;
            _j = 0;
            _innerLoopStarted = false;
            _keyInserted = true;
        }

        public bool PerformStep(int[] array, ref int comparisons, ref int swaps)
        {
            // Перевірка завершення сортування
            if (_i >= array.Length)
            {
                return true;
            }

            // Підготовка до нової ітерації зовнішнього циклу
            if (_keyInserted)
            {
                _key = array[_i];
                _j = _i - 1;
                _innerLoopStarted = false;
                _keyInserted = false;
            }

            // Внутрішній цикл - зсув елементів
            if (!_innerLoopStarted || (_j >= 0 && array[_j] > _key))
            {
                _innerLoopStarted = true;
                if (_j >= 0)
                {
                    comparisons++;
                    if (array[_j] > _key)
                    {
                        array[_j + 1] = array[_j];
                        swaps++;
                        _j--;
                    }
                    else
                    {
                        array[_j + 1] = _key;
                        _keyInserted = true;
                        _i++;
                    }
                }
                else
                {
                    array[0] = _key;
                    _keyInserted = true;
                    _i++;
                }
            }
            else
            {
                array[_j + 1] = _key;
                _keyInserted = true;
                _i++;
            }

            return false;
        }

        public (int, int, int) GetHighlightIndices()
        {
            return (_i, _j >= 0 ? _j : 0, -1);
        }
    }
}
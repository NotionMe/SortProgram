using System;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms
{
    public class BubbleSort
    {
        public static int[] Sort(int[] array)
        {
            int n = array.Length;
            int[] result = (int[])array.Clone();
            
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (result[j] > result[j + 1])
                    {
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
                return true;
            }

            if (_j < array.Length - _i - 1)
            {
                comparisons++;
                
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
                _j = 0;
                _i++;
            }

            return false;
        }

        public (int, int, int) GetHighlightIndices()
        {
            return (_j, _j + 1, -1);
        }
    }
}
using System;
using System.Collections.Generic;
using Practika2_OPAM_Ubohyi_Stanislav.ViewModels;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms
{
    public class QuickSort
    {
        public static int[] Sort(int[] array)
        {
            int[] result = (int[])array.Clone();
            QuickSortRecursive(result, 0, result.Length - 1);
            return result;
        }
        
        private static void QuickSortRecursive(int[] array, int low, int high)
        {
            if (low < high)
            {
                // Розподіляємо масив і отримуємо індекс опорного елемента
                int pivotIndex = Partition(array, low, high);
                
                // Рекурсивно сортуємо елементи перед опорним елементом і після нього
                QuickSortRecursive(array, low, pivotIndex - 1);
                QuickSortRecursive(array, pivotIndex + 1, high);
            }
        }
        
        private static int Partition(int[] array, int low, int high)
        {
            // Вибираємо опорний елемент (останній)
            int pivot = array[high];
            
            // Індекс меншого елемента
            int i = low - 1;
            
            for (int j = low; j < high; j++)
            {
                // Якщо поточний елемент менше або рівний опорному
                if (array[j] <= pivot)
                {
                    i++;
                    
                    // Міняємо елементи місцями
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
            
            // Міняємо array[i+1] і опорний елемент (array[high])
            int temp1 = array[i + 1];
            array[i + 1] = array[high];
            array[high] = temp1;
            
            return i + 1;
        }
    }

    public class QuickSortStrategy : ISortingStrategy
    {
        private Stack<(int, int)> _stack = new Stack<(int, int)>();
        private int _left = 0;
        private int _right = 0;
        private int _pivot = 0;
        private int _i = 0;
        private int _j = 0;
        private bool _partitioning = false;
        private int _index1 = -1;
        private int _index2 = -1;

        public void Initialize(int[] array)
        {
            _stack.Clear();
            _stack.Push((0, array.Length - 1));
            _partitioning = false;
            _index1 = -1;
            _index2 = -1;
        }

        public bool PerformStep(int[] array, ref int comparisons, ref int swaps)
        {
            if (_stack.Count == 0 && !_partitioning)
            {
                return true; // Сортування завершено
            }

            if (!_partitioning)
            {
                // Починаємо нове розділення
                (_left, _right) = _stack.Pop();
                
                // Якщо підмасив має розмір 1 або менше, він уже відсортований
                if (_left >= _right)
                {
                    return _stack.Count == 0;
                }
                
                // Вибираємо опорний елемент (останній елемент підмасиву)
                _pivot = array[_right];
                _i = _left - 1;
                _j = _left;
                _partitioning = true;
            }

            // Процес розділення
            if (_j < _right)
            {
                comparisons++;
                
                if (array[_j] <= _pivot)
                {
                    _i++;
                    
                    // Міняємо місцями елементи
                    if (_i != _j)
                    {
                        swaps++;
                        int temp = array[_i];
                        array[_i] = array[_j];
                        array[_j] = temp;
                    }
                }
                
                _index1 = _i;
                _index2 = _j;
                
                _j++;
            }
            else
            {
                // Завершуємо розділення, поміщаючи опорний елемент на правильне місце
                swaps++;
                int temp = array[_i + 1];
                array[_i + 1] = array[_right];
                array[_right] = temp;
                
                int partitionIndex = _i + 1;
                
                _index1 = partitionIndex;
                _index2 = _right;
                
                // Додаємо лівий та правий підмасиви у стек для подальшого сортування
                if (partitionIndex + 1 < _right)
                {
                    _stack.Push((partitionIndex + 1, _right));
                }
                
                if (_left < partitionIndex - 1)
                {
                    _stack.Push((_left, partitionIndex - 1));
                }
                
                _partitioning = false;
            }

            return false;
        }

        public (int, int, int) GetHighlightIndices()
        {
            return (_index1, _index2, -1);
        }
    }
}
using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms
{
    public class QuickSort
    {
        public static int[] Sort(int[] array)
        {
            int[] result = (int[])array.Clone(); // Копіюємо масив, щоб не змінювати оригінал
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
} 
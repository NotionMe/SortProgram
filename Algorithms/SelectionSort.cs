using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms
{
    public class SelectionSort
    {
        public static int[] Sort(int[] array)
        {
            int n = array.Length;
            int[] result = (int[])array.Clone(); // Копіюємо масив, щоб не змінювати оригінал
            
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
} 
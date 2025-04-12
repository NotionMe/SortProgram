using System;

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
} 
using System;

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
} 
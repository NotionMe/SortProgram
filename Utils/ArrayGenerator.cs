using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Utils
{
    public static class ArrayGenerator
    {
        private static Random random = new Random();
        
        // Генерація випадкового масиву
        public static int[] GenerateRandomArray(int size, int minValue = 1, int maxValue = 100)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(minValue, maxValue + 1);
            }
            return array;
        }
        
        // Генерація масиву, який майже відсортований
        public static int[] GenerateNearlySortedArray(int size, int swaps = 5)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = i + 1;
            }
            
            // Виконуємо кілька випадкових обмінів
            for (int i = 0; i < swaps; i++)
            {
                int index1 = random.Next(0, size);
                int index2 = random.Next(0, size);
                
                int temp = array[index1];
                array[index1] = array[index2];
                array[index2] = temp;
            }
            
            return array;
        }
        
        // Генерація масиву в зворотному порядку
        public static int[] GenerateReversedArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = size - i;
            }
            return array;
        }
        
        // Генерація масиву з багатьма повторюваними значеннями
        public static int[] GenerateFewUniqueArray(int size, int uniqueCount = 5)
        {
            int[] array = new int[size];
            int[] uniqueValues = new int[uniqueCount];
            
            // Генеруємо унікальні значення
            for (int i = 0; i < uniqueCount; i++)
            {
                uniqueValues[i] = random.Next(1, 100);
            }
            
            // Заповнюємо масив випадковими значеннями з унікальних
            for (int i = 0; i < size; i++)
            {
                array[i] = uniqueValues[random.Next(0, uniqueCount)];
            }
            
            return array;
        }
    }
} 
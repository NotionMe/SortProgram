using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms.Searching
{
    public class BinarySearchAlgorithmResult
    {
        public int Mid { get; set; }
        public bool Found { get; set; }
        public bool SearchInUpperHalf { get; set; } 
        public bool Completed { get; set; } 
    }

    public static class BinarySearchAlgorithm
    {
        public static BinarySearchAlgorithmResult SearchStep(int[] array, int valueToFind, int low, int high, ref int comparisons)
        {
            if (low > high || array.Length == 0)
            {
                return new BinarySearchAlgorithmResult { Completed = true, Found = false, Mid = -1 };
            }

            int mid = low + (high - low) / 2;
            comparisons++;

            if (array[mid] == valueToFind)
            {
                return new BinarySearchAlgorithmResult { Mid = mid, Found = true, Completed = true };
            }
            else if (array[mid] < valueToFind)
            {
                return new BinarySearchAlgorithmResult { Mid = mid, Found = false, Completed = false, SearchInUpperHalf = true };
            }
            else
            {
                return new BinarySearchAlgorithmResult { Mid = mid, Found = false, Completed = false, SearchInUpperHalf = false };
            }
        }
    }
}

using System;

namespace Practika2_OPAM_Ubohyi_Stanislav.Algorithms.Searching
{
    public class JumpSearchAlgorithmResult
    {
        public int CurrentPosition { get; set; }
        public int JumpStep { get; set; }
        public bool Found { get; set; }
        public bool Completed { get; set; }
        public bool IsJumpPhase { get; set; } // True під час пошуку блоку, False під час лінійного пошуку
    }

    public static class JumpSearchAlgorithm
    {
        public static JumpSearchAlgorithmResult SearchStep(int[] array, int valueToFind, int currentPos, int jumpStep, ref int comparisons)
        {
            if (array.Length == 0)
            {
                return new JumpSearchAlgorithmResult { 
                    Completed = true, 
                    Found = false, 
                    CurrentPosition = -1,
                    IsJumpPhase = false
                };
            }

            int n = array.Length;

            // Якщо це перший крок
            if (currentPos == -1)
            {
                jumpStep = (int)Math.Sqrt(n);
                return new JumpSearchAlgorithmResult {
                    CurrentPosition = 0,
                    JumpStep = jumpStep,
                    Found = false,
                    Completed = false,
                    IsJumpPhase = true
                };
            }

            // Перевіримо, чи поточний елемент є шуканим
            if (currentPos < n && array[currentPos] == valueToFind)
            {
                comparisons++;
                return new JumpSearchAlgorithmResult {
                    CurrentPosition = currentPos,
                    JumpStep = jumpStep,
                    Found = true,
                    Completed = true,
                    IsJumpPhase = false
                };
            }

            // Фаза стрибка - шукаємо блок
            if (currentPos < n && array[currentPos] < valueToFind && currentPos + jumpStep < n)
            {
                comparisons++;
                int nextPosition = currentPos + jumpStep;
                
                return new JumpSearchAlgorithmResult {
                    CurrentPosition = nextPosition,
                    JumpStep = jumpStep,
                    Found = false,
                    Completed = false,
                    IsJumpPhase = true
                };
            }
            
            // Якщо ми досягли кінця масиву в фазі стрибка або знайшли блок з більшим елементом
            else if (currentPos < n)
            {
                comparisons++;
                
                // Якщо ми в фазі стрибка і знайшли блок з потенційним елементом
                if (array[currentPos] > valueToFind)
                {
                    // Переходимо до лінійного пошуку, починаючи з попереднього блоку
                    int prevBlockStart = Math.Max(0, currentPos - jumpStep);
                    
                    return new JumpSearchAlgorithmResult {
                        CurrentPosition = prevBlockStart,
                        JumpStep = jumpStep,
                        Found = false,
                        Completed = false,
                        IsJumpPhase = false
                    };
                }
                else
                {
                    // Якщо це лінійний пошук - переходимо до наступного елементу
                    currentPos++;
                    
                    // Перевіряємо, чи не вийшли за межі блоку або масиву
                    int blockStart = Math.Max(0, currentPos - (currentPos % jumpStep));
                    int blockEnd = Math.Min(blockStart + jumpStep, n);
                    
                    if (currentPos >= n || currentPos >= blockEnd)
                    {
                        // Досягнуто кінця блоку або масиву
                        return new JumpSearchAlgorithmResult {
                            CurrentPosition = -1,
                            JumpStep = jumpStep,
                            Found = false,
                            Completed = true,
                            IsJumpPhase = false
                        };
                    }
                    
                    return new JumpSearchAlgorithmResult {
                        CurrentPosition = currentPos,
                        JumpStep = jumpStep,
                        Found = false,
                        Completed = false,
                        IsJumpPhase = false
                    };
                }
            }
            
            // Якщо ми дійшли до цього місця, повертаємо "не знайдено"
            return new JumpSearchAlgorithmResult {
                CurrentPosition = -1,
                JumpStep = jumpStep,
                Found = false,
                Completed = true,
                IsJumpPhase = false
            };
        }
    }
}
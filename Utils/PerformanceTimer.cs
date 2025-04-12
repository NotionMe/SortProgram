
            using System;
using System.Diagnostics;

namespace Practika2_OPAM_Ubohyi_Stanislav.Utils
{
    public class PerformanceTimer
    {
        private Stopwatch stopwatch;
        
        public PerformanceTimer()
        {
            stopwatch = new Stopwatch();
        }
        
        public void Start()
        {
            stopwatch.Reset();
            stopwatch.Start();
        }
        
        public void Stop()
        {
            stopwatch.Stop();
        }
        
        public TimeSpan ElapsedTime => stopwatch.Elapsed;
        
        public long ElapsedMilliseconds => stopwatch.ElapsedMilliseconds;
        
        public double ElapsedSeconds => stopwatch.Elapsed.TotalSeconds;
        
        // Вимірювання часу виконання алгоритму сортування
        public static PerformanceResult MeasureSortingAlgorithm<T>(Func<T[], T[]> sortingMethod, T[] array)
        {
            var timer = new PerformanceTimer();
            timer.Start();
            
            var result = sortingMethod(array);
            
            timer.Stop();
            
            return new PerformanceResult
            {
                ElapsedTime = timer.ElapsedTime,
                ElapsedMilliseconds = timer.ElapsedMilliseconds,
                ElapsedSeconds = timer.ElapsedSeconds
            };
        }
    }
    
    public class PerformanceResult
    {
        public TimeSpan ElapsedTime { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public double ElapsedSeconds { get; set; }
    }
} 
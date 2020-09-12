using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace PartitionTest
{
    public static class Counter
    {
        public static int ActivityNum;
        public static int OrchestrationNum;
        private static Timer ATimer;
        private static readonly Stopwatch stopwatch = new Stopwatch();

        public static void Start()
        {
            stopwatch.Start();
            ATimer = new Timer();
            ATimer.Elapsed += delegate
            {
                DateTime b = DateTime.Now;
                long senconds = stopwatch.ElapsedMilliseconds / 1000;

                if (senconds > 0)
                {
                    Console.WriteLine($"{senconds},{ActivityNum },qps = {(double)ActivityNum / senconds}");
                }
            };
            ATimer.Interval = 5000;
            ATimer.Enabled = true;
        }

    }
}

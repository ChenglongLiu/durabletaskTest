using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PartitionTest
{
    public static class Counter
    {
        public static int ActivityNum;
        public static int OrchestrationNum;
        private static DateTime StartTime;
        private static Timer ATimer;

        public static void Start()
        {
            StartTime = DateTime.Now;

            ATimer = new Timer();

            ATimer.Elapsed += delegate
            {
                DateTime b = DateTime.Now;
                int senconds = (int)b.Subtract(StartTime).TotalSeconds;

                if (senconds > 0)
                {
                    Console.WriteLine($"{senconds},{ActivityNum },{ActivityNum / senconds}");
                }
            };
            ATimer.Interval = 5000;
            ATimer.Enabled = true;
        }

    }
}

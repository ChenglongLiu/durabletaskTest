using System;
using System.Threading;
using DurableTask.Core;
using System.Threading.Tasks;

namespace CombineTimerTest
{
    public class HelloOrchestration : TaskOrchestration<string, string>
    {
        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            var instance = context.OrchestrationInstance;
            
            using var ctsTimer = new CancellationTokenSource();
            using var ctsTask = new CancellationTokenSource();
            DateTime deadline = context.CurrentUtcDateTime + TimeSpan.FromSeconds(20);
            
            Task<bool> activityTask = context.ScheduleTask<bool>(typeof(HelloTask));
            Task timeoutTask = context.CreateLongTimer<string>(deadline, null, ctsTimer.Token);

            Task winner = await Task.WhenAny(activityTask, timeoutTask);
            if (winner == activityTask)
            {
                // success case
                Console.WriteLine("ActivityTask finished");
                ctsTimer.Cancel();
                await activityTask;
                return "Succeeded";
            }
            else
            {
                // timeout case
                ctsTask.Cancel();
                return "Timeout";
            }
        }


        
    }
}
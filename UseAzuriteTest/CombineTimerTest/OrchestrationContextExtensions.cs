using System;
using System.Threading;
using System.Threading.Tasks;
using DurableTask.Core;

namespace CombineTimerTest
{
    public static class OrchestrationContextExtensions
    {
        /// <summary>
        ///     Create a long timer that will fire at the specified time and hand back the specified state.
        /// </summary>
        /// <typeparam name="T">Type of state object</typeparam>
        /// <param name="context"></param>
        /// <param name="fireAt">Absolute time at which the timer should fire</param>
        /// <param name="state">The state to be handed back when the timer fires</param>
        /// <param name="cancelToken">Cancellation token</param>
        /// <returns>Task that represents the async wait on the timer</returns>
        public static async Task<T> CreateLongTimer<T>(this OrchestrationContext context, DateTime fireAt, T state, CancellationToken cancelToken)
        {
            var maxTimeSpan = TimeSpan.FromSeconds(10);
            while (context.CurrentUtcDateTime < fireAt)
            {
                if (fireAt - context.CurrentUtcDateTime >= maxTimeSpan)
                {
                    Console.WriteLine($"Wait until {context.CurrentUtcDateTime.Add(maxTimeSpan)}");
                    await context.CreateTimer<string>(context.CurrentUtcDateTime.Add(maxTimeSpan), null, cancelToken);
                }
                else
                {
                    Console.WriteLine($"Wait until {fireAt}");
                    await context.CreateTimer<string>(fireAt, null, cancelToken);
                }
            }

            return state;
        }
        
    }
}
using System;
using System.Threading.Tasks;
using DurableTask.Core;

namespace CombineTimerTest
{
    public class HelloTask: AsyncTaskActivity<string, bool>
    {
        protected override async Task<bool> ExecuteAsync(TaskContext context, string input)
        {
            await Task.Delay(TimeSpan.FromSeconds(125));
            return true;
        }
    }
}
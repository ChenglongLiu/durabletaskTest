using System;
using System.Threading.Tasks;
using DurableTask.Core;

namespace SessionDataTest
{
    public class HelloTask: AsyncTaskActivity<string, bool>
    {
        protected override async Task<bool> ExecuteAsync(TaskContext context, string input)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(2));
            return true;
        }
    }
}
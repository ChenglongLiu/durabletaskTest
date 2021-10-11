using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace SessionDataTest
{
    public class HelloOrchestration : TaskOrchestration<string, string>
    {
        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            var instance = context.OrchestrationInstance;
            await context.ScheduleTask<bool>(typeof(HelloTask));
            
            return "Succeeded";
        }
    }
}
using System;
using System.Threading.Tasks;
using DurableTask.Core;

namespace CreateRetryableClient
{
    public class HelloOrchestration : TaskOrchestration<string, string>
    {
        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            var instance = context.OrchestrationInstance;
            
            var retryOptions = new RetryOptions(TimeSpan.FromSeconds(10), 10)
            {
                BackoffCoefficient = 2,
                MaxRetryInterval = TimeSpan.FromHours(1),

                Handle = (e) =>
                {
                    Console.WriteLine(DateTime.Now + " : " + e.Message);
                    return true;
                }
            };
            
            var failedTask = context.CreateRetryableClient<IFailedTask>(retryOptions);
            await failedTask.TaskThrowEx();
            return "Succeeded";
        }
    }
}
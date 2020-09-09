using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DurableTask.Core;

namespace PartitionTest
{
    class Test
    {
        public static async Task CreateMockOrchestrationInstancesAsync(int instanceNum, int activityNum, TaskHubClient client)
        {
            for(int i = 0; i < instanceNum; i++)
            {
                await client.CreateOrchestrationInstanceAsync(typeof(MockOrchestration), activityNum);
            }
        }

        public static async Task StartWorker(TaskHubWorker worker)
        {
            worker.AddTaskOrchestrations(typeof(MockOrchestration));
            worker.AddTaskActivities(new MockActivity());
            await worker.StartAsync();

            Counter.Start();
        }
    }
}

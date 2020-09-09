using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DurableTask.Core;

namespace PartitionTest
{
    class MockOrchestration : TaskOrchestration
        <bool, int>
    {
        public override async Task<bool> RunTask(OrchestrationContext context, int num)
        {
            for(int i = 0; i < num; i++)
            {
                await context.ScheduleTask<string>(typeof(MockActivity), "");
            }
            Counter.OrchestrationNum++;
            return true;
        }
    }
}

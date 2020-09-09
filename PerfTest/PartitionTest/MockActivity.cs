using System;
using System.Collections.Generic;
using System.Text;
using DurableTask.Core;

namespace PartitionTest
{
    class MockActivity : TaskActivity
    {
        public override string Run(TaskContext context, string input)
        {
            Counter.ActivityNum++;
            return "";
        }
    }
}

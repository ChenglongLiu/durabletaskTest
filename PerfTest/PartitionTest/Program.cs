using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Common;
using DurableTask.AzureStorage;
using DurableTask.Core;
using DurableTask.Core.Tracing;

namespace PartitionTest
{
    class Program
    {
        class CustomEventListener : EventListener
        {
            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                Console.WriteLine(eventData);
            }
        }


        static async Task Main(string[] args)
        {
            var listener = new CustomEventListener();
            listener.EnableEvents(DefaultEventSource.Log, EventLevel.Warning);

            string storageConnectionString = Storage.StorageConnectionString;
            var settings = new AzureStorageOrchestrationServiceSettings
            {
                StorageConnectionString = storageConnectionString,
                //TaskHubName = "PartitionTest",
                TaskHubName = "PartitionTest",
                PartitionCount = 1,
                //MaxConcurrentTaskOrchestrationWorkItems = 100,
                //MaxConcurrentTaskActivityWorkItems = 10,
                //MaxStorageOperationConcurrency = Environment.ProcessorCount * 25,
            };

            var orchestrationServiceAndClient = new AzureStorageOrchestrationService(settings);
            //just for test
            await orchestrationServiceAndClient.DeleteAsync();

            //await Task.Delay(TimeSpan.FromSeconds(60));
            await orchestrationServiceAndClient.CreateIfNotExistsAsync();

            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient);
            var taskHubWorker = new TaskHubWorker(orchestrationServiceAndClient);

            //1.create 1000** instance
            await Test.CreateMockOrchestrationInstancesAsync(40, 5, taskHubClient);

            //
            await Test.StartWorker(taskHubWorker);

            Console.WriteLine("Press any key to quit.");
            Console.ReadLine();
        }
    }
}

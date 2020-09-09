using System;
using System.Threading.Tasks;
using DurableTask.AzureStorage;
using DurableTask.Core;

namespace PartitionTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string storageConnectionString = "";
            var settings = new AzureStorageOrchestrationServiceSettings
            {
                StorageConnectionString = storageConnectionString,
                //TaskHubName = "PartitionTest",
                TaskHubName = "PartitionTest",
                PartitionCount = 16,
            };

            var orchestrationServiceAndClient = new AzureStorageOrchestrationService(settings);
            //just for test
            await orchestrationServiceAndClient.DeleteAsync();
            await Task.Delay(TimeSpan.FromSeconds(60));
            await orchestrationServiceAndClient.CreateIfNotExistsAsync();

            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient);
            var taskHubWorker = new TaskHubWorker(orchestrationServiceAndClient);


            //1.create 1000** instance
            await Test.CreateMockOrchestrationInstancesAsync(200, 10, taskHubClient);

            //
            await Test.StartWorker(taskHubWorker);

            Console.WriteLine("Press any key to quit.");
            Console.ReadLine();
        }
    }
}

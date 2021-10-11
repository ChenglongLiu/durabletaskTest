using DurableTask.AzureStorage;
using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace SessionDataTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Use Azurite emulator
            string storageConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" + 
                                             "BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1";
            string taskHubName = "Hello";
            
            var settings = new AzureStorageOrchestrationServiceSettings();
            settings.StorageConnectionString = storageConnectionString;
            settings.TaskHubName = taskHubName;
            settings.UseDataContractSerialization = true;
            
            var orchestrationServiceAndClient = new AzureStorageOrchestrationService(settings);
            await orchestrationServiceAndClient.CreateIfNotExistsAsync();

            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient);
            var taskHub = new TaskHubWorker(orchestrationServiceAndClient);
            
            // add instance
            _ = Task.Run(async () =>
            {
                OrchestrationInstance ins = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(HelloOrchestration), "");
            });

            // add worker
            try
            {
                taskHub.AddTaskOrchestrations(
                    typeof(HelloOrchestration)
                    );

                taskHub.AddTaskActivities(typeof(HelloTask));
                taskHub.AddOrchestrationDispatcherMiddleware(async (context, next) =>
                {
                    OrchestrationRuntimeState runtimeState = context.GetProperty<OrchestrationRuntimeState>();
                    var customInstance = OrchestrationInstanceEx.Initialize(runtimeState);
                    customInstance.Dic["a"] = "a";
                    customInstance.Dic["b"] = "b";
                    customInstance.Dic["c"] = "c";
                    context.SetProperty<OrchestrationInstance>(customInstance);
                    
                    await next();
                });
                
                taskHub.AddOrchestrationDispatcherMiddleware(async (context, next) =>
                {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
                    var customInstance = OrchestrationInstanceEx.Get(context.GetProperty<OrchestrationInstance>());
                    context.SetProperty<OrchestrationInstance>(customInstance);
                    
                    //Dic data can get here. But missed in HelloOrchestration context and ActivityDispatcherMiddleware
                    
                    await next();
                });

                taskHub.AddActivityDispatcherMiddleware(async (context, next) =>
                {
                    var customInstance = OrchestrationInstanceEx.Get(context.GetProperty<OrchestrationInstance>());
                    context.SetProperty<OrchestrationInstance>(customInstance);
                    
                    //Dic data missed
                    
                    await next();
                });

                await taskHub.StartAsync();
                //Console.WriteLine("Press any key to quit.");
                Console.ReadLine();
                taskHub.StopAsync(true).Wait();
            }
            catch (Exception e)
            {
                // silently eat any unhandled exceptions.
                Console.WriteLine($"worker exception: {e}");
            }
        }
    }
}
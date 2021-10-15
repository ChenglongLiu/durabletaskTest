using System;
using System.Threading.Tasks;

namespace CreateRetryableClient
{
    public class FailedTask : IFailedTask
    {
        public Task TaskThrowEx()
        {
            Console.WriteLine(DateTime.Now + " : Run TaskThrowEx");
            throw new System.NotImplementedException();
        }
    }
}
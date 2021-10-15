using System.Threading.Tasks;

namespace CreateRetryableClient
{
    public interface IFailedTask
    {
        public Task TaskThrowEx();

    }
}
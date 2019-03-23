using System.Threading.Tasks;

namespace ImagesWorkerAPI.Interfaces
{
    public interface IQueueService
    {
        Task AddMessageAsync(string message);
    }
}

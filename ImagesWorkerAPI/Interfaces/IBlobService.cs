using System.IO;
using System.Threading.Tasks;

namespace ImagesWorkerAPI.Interfaces
{
    public interface IBlobService
    {
        Task UploadBlobAsync(Stream stream, string fileName);
        Task<byte[]> GetBlobByUrlAsync(string url);
    }
}

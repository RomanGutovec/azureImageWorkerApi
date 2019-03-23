using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImagesWorkerAPI.Interfaces
{
    public interface IBlobService
    {
        Task UploadBlobAsync(Stream stream, string fileName);
        Task<byte[]> GetBlobByUrlAsync(string url);
    }
}

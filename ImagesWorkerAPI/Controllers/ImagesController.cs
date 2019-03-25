using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImagesWorkerAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;

namespace ImagesWorkerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IBlobService service;
        private readonly IQueueService queueService;
        public ImagesController(IBlobService service, IQueueService queueService)
        {
            this.service = service;
            this.queueService = queueService;
        }

        // POST: api/Images
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Post(IFormFile uploadedFile)
        {
            Logger.Info($"Start post image to the server.");
            try
            {
            await service.UploadBlobAsync(uploadedFile.OpenReadStream(), uploadedFile.FileName);

            await queueService.AddMessageAsync(uploadedFile.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Some error occured while posting image.");
                throw;
            }

            return Ok();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Logger.Info($"Start getting image with id: {id}.");

            try
            {

            var byteArray = await service.GetBlobByUrlAsync(id);

                if (byteArray == null)
                {
                    Logger.Info($"No image with id {id} was found.");
                    return NotFound();
                }

                Logger.Info($"Retrieving image with id {id} to response.");

                using (var stream = new MemoryStream(byteArray))
                {
                    return File(stream, "image/jpeg");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Some error occured while getting image.");
                throw;
            }
        }        
    }
}

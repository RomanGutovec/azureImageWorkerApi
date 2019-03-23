using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImagesWorkerAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImagesWorkerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
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
            await service.UploadBlobAsync(uploadedFile.OpenReadStream(), uploadedFile.FileName);

            await queueService.AddMessageAsync(uploadedFile.FileName);

            return Ok();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {            
            var byteArray = await service.GetBlobByUrlAsync(id);
            var stream = new MemoryStream(byteArray);

            return File(stream, "image/jpeg");
        }        
    }
}

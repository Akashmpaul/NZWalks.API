using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageDto imageDto)
        {
            ValidateFileUpload(imageDto);

            if(ModelState.IsValid) 
            {
                // Convert DTO to domain model
                var imageDomainModel = new Image
                {
                    File = imageDto.File,
                    FileExtension = Path.GetExtension(imageDto.File.FileName),
                    FileSizeInByte = imageDto.File.Length,
                    FileName = imageDto.File.FileName,
                    FileDescription = imageDto.FileDescription

                };

                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }


        private void ValidateFileUpload(ImageDto imageDto)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if(!allowedExtensions.Contains(Path.GetExtension(imageDto.File.FileName))) 
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if(imageDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, Please upload a smaller size file");
            }
        }
    }
}

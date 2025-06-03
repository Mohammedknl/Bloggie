using System.Net;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    //This is a Web API controller and it doesn't return View back it returns responses like 200 for success,400 for bad request and 500 for something error
    //https://localhost:xxx/api/Images
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {


        private readonly IImageRespository imageRespository;

        public ImagesController(IImageRespository imageRespository)
        {
            this.imageRespository = imageRespository;
        }
        /*This is just for verfication
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("This method is to Test the response");
        }
        */
        //This is the post method which takes the Image and upload the image to cloudhosting platform(cloudinary) using Repository
        [HttpPost]
        //ASP will receive an image file in the form of IFormFile with name as file
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            // calling a imagerepository
            var imageURL = await imageRespository.UploadAsync(file);

            if (imageURL == null) //If something error happend ie image is null
            {
                return Problem("Sometihng went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }
            //If the imageURL is available ie getting string back then return JSON result with link
            return new JsonResult(new { link = imageURL });
        }
    }
}

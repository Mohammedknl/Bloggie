using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    //This API controller is for BlogPostLike functionality when user cliks on Icon and adding JS code inside Blogs/Index.cshtml
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.blogPostLikeRepository = blogPostLikeRepository;
        }
        //Creating a method for adding a like using AddLike with a request object AddLikeRequest as a parameter
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            //Here using BlogPostLike Repository methods to perform Like Functionality and used inside JS code of index.cshtml
            var model = new BlogPostLike
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId
            };

            await blogPostLikeRepository.AddLikeForBlog(model);

            return Ok(); //It is a 200 success response for an API
        }

        //This method is for getting Total no of likes based on BlogPost ID and used inside JS code of Index.cshtml
        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
        {
            var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPostId);

            return Ok(totalLikes);
        }
    }
}

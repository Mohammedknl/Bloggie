using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet] //This will take route as input parameter
        public async Task<IActionResult> Index(string urlHandle)
        {
            //Using this Url Handle we will find the blog based on urlhandle search
           // var liked = false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            //Below is to check the Total count of Likes for particular Blog
            //var blogDetailsViewModel = new BlogDetailsViewModel();
            /*
            if (blogPost != null)
            {
                var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (signInManager.IsSignedIn(User))
                {
                    // Get like for this blog for this user
                    var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
            */
                    return View(blogPost);
        }
    }
}


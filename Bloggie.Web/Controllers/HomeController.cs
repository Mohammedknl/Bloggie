using System.Diagnostics;
using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger,
            IBlogPostRepository blogPostRepository,
            ITagRepository tagRepository
            )
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }

        //public async Task<IActionResult> Index()
        //{
        //    // getting all blogs via blogPostRepository
        //    var blogPosts = await blogPostRepository.GetAllAsync();

        //    // get all tags via tagRepository
        //    var tags = await tagRepository.GetAllAsync();
        //    //Model binding and displaying the result back to views
        //    var model = new HomeViewModel
        //    {
        //        BlogPosts = blogPosts,
        //        Tags = tags
        //    };

        //    return View(model);
        //}

        //Below is the updated method for showing Tags and Blogs based on user diff Tag clicks 
        public async Task<IActionResult> Index(string? tag)
        {
            // Get all blog posts
            var blogPosts = await blogPostRepository.GetAllAsync();

            // If a tag is selected, filter blog posts by that tag
            if (!string.IsNullOrWhiteSpace(tag))
            {
                blogPosts = blogPosts.Where(bp => bp.Tags.Any(t => t.Name == tag)).ToList();
                ViewBag.SelectedTag = tag; // Optional: to highlight the selected tag in the view
            }

            // Sort BlogPost by latest date (most recent blogs first)
            blogPosts = blogPosts
                .OrderByDescending(bp => bp.PublishedDate)
                .ToList();

            // Get all tags
            var tags = await tagRepository.GetAllAsync();

            // Prepare the view model
            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(model);
        }

        //About Us page code added here
        public IActionResult AboutUs()
        {
            return View();
        }


        //Old code below
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

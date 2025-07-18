﻿using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository,IBlogPostLikeRepository blogPostLikeRepository,SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        
        //This Get method will show the Blog details on the page
        [HttpGet] //This will take route as input parameter
        public async Task<IActionResult> Index(string urlHandle)
        {
            //Using this Url Handle we will find the blog based on urlhandle search
            var liked = false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            var blogDetailsViewModel = new BlogDetailsViewModel();
            



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


                }

                // Get comments/populate comments for all blog post about information like Description DateAdded and Username
                //var blogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

// Get comments/populate comments for all blog post about information like Description DateAdded and Username in descending order by date
                var blogCommentsDomainModel = (await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id))
                                              .OrderByDescending(c => c.DateAdded);
                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
                }

                //Now converting a Domain model to BlogDetails View Model 
                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    //Assigning each property from Domain Model to DetailsView Model
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView

                };
            }

              return View(blogDetailsViewModel);
        }
        //This below method will post the comments from blogs to DB
        //For comments functionality
        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs",
                    new { urlHandle = blogDetailsViewModel.UrlHandle });
            }

            return View();
        }
    }
}


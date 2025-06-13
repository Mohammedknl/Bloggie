
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    //This is the Class to implement IBlogPostRepository Interface
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        //Injecting DBContext to talk to DB Table using COnstructor
        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        //Below is the implementation of Interface method GetTotalLikes
        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            //Returns total no of likes based on the BlogPost ID
            return await bloggieDbContext.BlogPostLike.CountAsync(x=>x.BlogPostId==blogPostId);
        }

        //Implementing AddLikeForBlog Method to perform JS action on Like action used in BlogPostLikeCOntroller
        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await bloggieDbContext.BlogPostLike.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        //Get likes for blog for particular user to get list of all likes for the Blogpost this is used inside BlogsController.cs
        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await bloggieDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId)
                .ToListAsync();
        }
    }
}

using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    //This is an Interface where we only define the methods for BlogPost Like Functionality
    public interface IBlogPostLikeRepository
    {
        //This method return Total likes based on BlogPost ID as a parameter implemented inside repository andused inside BlogsController
        Task<int> GetTotalLikes(Guid blogPostId);
        //This below method is implemented inside repository and used inside BlogPostLikeCOntroller.cs
        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
        //This method is to check which user has liked how many times
        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
    }
}

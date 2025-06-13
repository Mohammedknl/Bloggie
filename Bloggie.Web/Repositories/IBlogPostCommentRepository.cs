using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    //This Interface is for Comment functionality 
    public interface IBlogPostCommentRepository
    {
        //Creating structure for Adding Blogs comment 
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);

        //Creating structure for Displaying Comments  using GetCommentsByIdAsync emthod
        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId);
    }
}

namespace Bloggie.Web.Models.Domain
{
    //This Domain model is for Comment functionality with 4 properties
    public class BlogPostComment
    {
        public Guid Id { get; set; }
        //For writing comments for blogs
        public string Description { get; set; }
        //for which blog is this comment
        public Guid BlogPostId { get; set; }
        //which user commented to this blog
        public Guid UserId { get; set; }
        //Time and Data this comment was added
        public DateTime DateAdded { get; set; }
    }
}

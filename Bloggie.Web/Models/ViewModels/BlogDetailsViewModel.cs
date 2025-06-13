using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    //This View Model is for BlogPostLike functionality to display all properties 
    public class BlogDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public ICollection<Tag> Tags { get; set; }
        //Adding a new property for Total Likes
        public int TotalLikes { get; set; }
        //New property for user liked or not
        public bool Liked { get; set; }
        //This prop is for Comment functionlaity used inside BlogsController.cs
        public string CommentDescription { get; set; }

        //For displaying all Comments for particular blogs
        public IEnumerable<BlogComment> Comments { get; set; }
    }
}

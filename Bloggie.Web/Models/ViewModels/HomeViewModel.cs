using Bloggie.Web.Models.Domain;
namespace Bloggie.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        //propety for list of Blogs
        public IEnumerable<BlogPost> BlogPosts { get; set; }
        //property for list of Tags
        public IEnumerable<Tag> Tags { get; set; }
    }
}

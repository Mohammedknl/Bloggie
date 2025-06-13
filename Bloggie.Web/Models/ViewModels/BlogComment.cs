namespace Bloggie.Web.Models.ViewModels
{
    //This view model is for Displaying Comments for each blogs
    public class BlogComment
    {
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string Username { get; set; }
    }
}

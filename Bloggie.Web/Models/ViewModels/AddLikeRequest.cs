namespace Bloggie.Web.Models.ViewModels
{
    public class AddLikeRequest
    {
        //This will have two Id's like Blogpost ID and who is Liking with UserID
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}

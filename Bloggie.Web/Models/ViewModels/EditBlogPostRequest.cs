using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Models.ViewModels
{
    public class EditBlogPostRequest
    {
//This View model is for Editing or updating the Blogpost based on Id 
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


        // Also Display tags
        public IEnumerable<SelectListItem> Tags { get; set; }
        // Also want to show all Collect Tags
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddBlogPostRequest
    {
        //Copied all properties from Domain Model Except ID it is by default added
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        //Adding 2 properties for displaying tags and other for capture the tags 

        // For Displaying tags inside a dropdown list on Add page
        public IEnumerable<SelectListItem>Tags { get; set; }
        //prop for Collecting Multiple Tags for each Blog
        //changing a dropdown selector fro collecting one tag in to multiple tags select dropdown user can collect multiple tags for a single Blog
        //Converting dropdown selector in to an string array
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
        
    }
}

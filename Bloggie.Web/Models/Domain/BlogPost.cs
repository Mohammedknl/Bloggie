//Created BlogPost Model class inside Domain Model

namespace Bloggie.Web.Models.Domain
{
    public class BlogPost
    {
        //Start creating properties for Domain Model and these properties can be used throughout the application as it is public
        //To create a property type prop and press Tab/enter
        //***Below are the properties for BlogPost Model****
       //public is access specifier GUID is the type of the property and ID is name of the property and getters and setters for DB tables
        public Guid Id { get; set; }  //GUID is the unique Identity property in dotnet

        //public string? Heading { get; set; } // If we put ? before type of property then it can accept Null values in DB
        
        //Here all the properties are non nullable means it shuld not contain Null values before saving in to DB
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        //**As BlogPost can have multiple Tags and Tags can have multiple Tags
        //so we need to create many to many relationship between BlogPost and Tag models like below using ICollection
        //This is to inform EF core that this BlogPost can have multiple Tags with the below lines

        // Navigation property for Tags from BlogPost using Icollection type and Tag is the Model name and Tags is the Name of the property
       //Here we r saying that a BlogPost can have multiple Tags
        public ICollection<Tag> Tags { get; set; }
        
        //To get the total no of likes to the BlogPost
        public ICollection<BlogPostLike> Likes { get; set; }

        //Linking BlogPostComment property ie one blog post can have multiple comments
        public ICollection<BlogPostComment> Comments { get; set; }

        



    }
}

//Creating a Tag Model class inside Domain Model with 3 properties GUID,Name and DisplayName
namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }


        //Here in Tag model also we can create Multiple relationship between Tag and BlogPost
        //we r telling EF core that a Tag can have multiple BlogPost 
        //Here also we use Icollection property with type as BlogPost which is model and name of property is BlogPosts
        public ICollection<BlogPost> BlogPosts { get; set; }


    }
}

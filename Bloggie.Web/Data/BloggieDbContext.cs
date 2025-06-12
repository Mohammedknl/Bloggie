using Bloggie.Web.Models.Domain; //This is for using EF Models for BlogPost and Tag, BlogPostLike and BlogpostComment
using Microsoft.EntityFrameworkCore;//from this package we r accessign DbContext class

namespace Bloggie.Web.Data
{
    public class BloggieDbContext : DbContext  //Inherting BloggieDbContext class from DbContext which is imported from EF core
    {
        //Here we have created a Constructor for BloggieDbContext with options as parameters by pressing ctrl+ . 
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
        {

        }
        //Creating properties for creating Tables inside Database using DbSet as type and Models name by typing prop + tab/enter
        public DbSet<BlogPost> BlogPosts { get; set; } //BlogPost is the Entity comes from Domain Models and BlogPosts is the name of the Table inside DB
        public DbSet<Tag> Tags { get; set; } //Here EF core is creating a Tags table with type as DbSet and using Tag Domain Model
       
        //To access the BlogPostLike Table
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        //public DbSet<BlogPostComment> BlogPostComment { get; set; }

       
    }
}

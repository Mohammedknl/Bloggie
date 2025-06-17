using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    //Here we write the implementation of Repository Interface inside this Repository class
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        //To implement we need bloggieDbContext to talk to DB Models
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
            
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost); // same inside controller await blogPostRepository.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;

        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            //first we will find whethere the blogpost is present or not inside DB based on ID
            var existingBlog = await bloggieDbContext.BlogPosts.FindAsync(id);
            //if blogpost found
            if (existingBlog != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            //Else if blogpost is not found in DB
            return null;
        }
        //This is for getting all Blogpost to display on List Page
        //public async Task<IEnumerable<BlogPost>> GetAllAsync()
        //{
        //    //This is to get all the Blogs from DB give back to controller to display on List view using bloggieDbContext
        //    return await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        //}

        //New code for GetAllAsync method implementation added here with Pagination

        public async Task<IEnumerable<BlogPost>> GetAllAsync(int pageNumber = 1,int pageSize = 100)
        {
            //Converting DBSet to a Querable for search Query
            var query = bloggieDbContext.BlogPosts.Include(x => x.Tags).AsQueryable();

            // Pagination is like skipping few results and accepting other results
            // Skip 0 Take 5 -> Page 1 of 5 results
            // Skip 5 Take next 5 -> Page 2 of 5 results
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();

          // return await bloggieDbContext.Tags.ToListAsync(); converting this line in to above 2 lines for pagination
        }
        //Below New method is for Counting total no of elements inside Tags table for Pagination functionality
        public async Task<int> CountAsync()
        {
            return await bloggieDbContext.BlogPosts.CountAsync();
        }


        //New code ends here

        //This is for getting a single Blog based on ID also by including Tags information used for Edit/Delete Action Methods
        //If found return Blogpost else returns null as we give ? for BlogPost?
        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            //First we will find if the blogpost is available with the ID
            var existingBlog = await bloggieDbContext.BlogPosts.Include(x => x.Tags)
               .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            //if particular Blog is found in DB
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }


    }
}

using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostRepository
    {
        //Here Inside Interface we only Define the methods and implemented inside IBlogPostRepository class
        Task<IEnumerable<BlogPost>> GetAllAsync(int pageNumber = 1,int pageSize = 100);

        Task<BlogPost?> GetAsync(Guid id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle); //Used for Displaying Blogpost based on url handle search

        Task<BlogPost> AddAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);

        //Below method is for pagination  to give the count of elements of table
        Task<int> CountAsync();


    }
}

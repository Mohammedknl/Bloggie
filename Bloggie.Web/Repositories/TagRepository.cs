using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    //Here we are going to implement the Methods which we define inside ITagInterface for performing CRUD operations
    // This is an empty class which inherits from ITagInterface class
    //To use this Inherit class I we need to implement all the methods which we define inside an Interface(Ctrl+. and add implement)
    public class TagRepository : ITagRepository

//To talk to DB inside this Repository class we need use of BlogieDbContect same like how e used inside controller
//And from the Controller methods we delete the code which we have to connect to dbContext and place it here inside repository
//For this we need a Constructot to use dbContext class and a private field same as Controller logic

    {
        private readonly BloggieDbContext bloggieDbContext;

        //This is the constructor where we inject a DbContext class and also uses the service from program.cs
        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        //Below are the methods which we need to implement inside the repository by copying the code from COntrollers
        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag); //making Database call and using await we make Asynchronous call

            await bloggieDbContext.SaveChangesAsync(); //writing in to DB

            return tag; //Returning newly create tag
        }

        public async Task<Tag> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                bloggieDbContext.Tags.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }

            return null;
        }


        //public async Task<IEnumerable<Tag>> GetAllAsync()
        //{
        //   return await bloggieDbContext.Tags.ToListAsync();
        //}
        //New code foe Implemeting GetAllAsync method to list all Tags based on Search query and pagination
        public async Task<IEnumerable<Tag>> GetAllAsync(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageNumber = 1,
            int pageSize = 100)
        {
            //Converting DBSet to a Querable for search Query
            var query = bloggieDbContext.Tags.AsQueryable();

            // Filtering on two diff columns ie on Name column and DisplayName column based on Query searh
            //If the searcch query is Null or have null values
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
               //Filtering on Name section and DisplayName section or columns using Contains method
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                                         x.DisplayName.Contains(searchQuery));
            }

            // Sorting ascending or Descending order 
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }

                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }
            }

            // Pagination is like skipping few results and accepting other results
            // Skip 0 Take 5 -> Page 1 of 5 results
            // Skip 5 Take next 5 -> Page 2 of 5 results
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();

            // return await bloggieDbContext.Tags.ToListAsync(); converting this line in to above 2 lines for filtering 
        }
        //Below New method is for Counting total no of elements inside Tags table for Pagination functionality
        public async Task<int> CountAsync()
        {
            return await bloggieDbContext.Tags.CountAsync();
        }



        public Task<Tag?> GetAsync(Guid id)
        {
            return bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);
            //If the existing tag is not null then update the Name and Display Name properties
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await bloggieDbContext.SaveChangesAsync();
                return existingTag;
            }
            //else if existing tag is null then return null method back
            return null;
        }
    }
}

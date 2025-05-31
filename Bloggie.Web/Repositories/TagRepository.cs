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


        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
           return await bloggieDbContext.Tags.ToListAsync();
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

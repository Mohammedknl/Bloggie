using Bloggie.Web.Models.Domain;

//Here we only write the definations of Action Methods inside Interface class ITagInterface
namespace Bloggie.Web.Repositories
{
//This is the Interface where we can only define the methods like how to access DB and access Tag tables from DB(not implementation)
    public interface ITagRepository
    {
        //Here we define 5 methods used inside AdminTags Controller for performing 5 CRUD operations like
        //Getting all tags, Getting a single Tag,Adding a new Tag,Updating Tag and Deleting Tag
        //As BloggieDbContext always knows about Domain Models ie Tag Domain model Tag is used to fetch the records/Tags
        //1.Creating Asynchronous method to get all tags back from DB for Asynchronus wrapping return type inside Task<>
        //IEnumerable is same as List of all tags from DB
        // Task<IEnumerable<Tag>> GetAllAsync();
        //New GetAllAsync method to list all Tags based on search and Pagination
        Task<IEnumerable<Tag>> GetAllAsync(
            string? searchQuery = null,
            string? sortBy = null,
            string? sortDirection = null,
            int pageNumber = 1,
            int pageSize = 100);

        //2.Creating Asynchronous method to get single tag back based on parameter Guid from DB for Asynchronus wrapping return type inside Task<>
        Task<Tag?> GetAsync(Guid id);

        //3.Creating Asynchronous method to Add/Create a new Tag in to  back from DB for Asynchronus wrapping return type inside Task<>
        //For creating new Tag AddAsync method will take complete Tag object itself as a parameter to save in to DB
        Task<Tag>AddAsync(Tag tag);

        //4.Creating Asynchronous method to update tag in to  DB for Asynchronus wrapping return type inside Task<>
        //Tag can be a Tag or null as it should accept null for updating Tag
        Task<Tag?> UpdateAsync(Tag tag);

        //1.Creating Asynchronous method to Deletea  tag from DB using Guid as a parameter for Asynchronus wrapping return type inside Task<>
        Task<Tag> DeleteAsync(Guid id);

        //Below method is for pagination  to give the count of elements of table
        Task<int> CountAsync();


    }
}

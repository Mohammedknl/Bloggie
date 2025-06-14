using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repositories
{
    public interface IUserRepository
    {
        //Method defination to get all Users list
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}

using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    //Implementing IUserRepository Interface method using AuthDBContext we can fetch the Users list
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;

        //Constructor to inject AuthDb Context for talking to DB whihc can be used inside GetAll method
        public UserRepository(AuthDbContext authDbContext) 
        {
            this.authDbContext = authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            //Below is to fetch all the users from the Auth DB
            var users = await authDbContext.Users.ToListAsync();
            //Checking for SuperAdmin User not to show the Superadmin details(ie to remove the Superadmin from users list)
            var superAdminUser = await authDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "superadmin@bloggie.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }

            return users;
        }
    }
}

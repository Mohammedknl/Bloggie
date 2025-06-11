using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //S1.Seeding list of roles ie (User,Admin,Superadmin)
            
            //below are GUID for diff roles
            var adminRoleId = "2d328e9c-0e6f-4dde-a348-21da5d81916f";
            var superAdminRoleId = "c5d93f3e-06a8-4b8e-ba81-7b719a4c2e5e";
            var userRoleId = "df5a8f2a-b01c-4427-beb6-d7345128cf0b";
            //creating list of roles before seeding roles using class IdentityRole by Microsoft Identity package
            var roles = new List<IdentityRole>
            {
                //Creating first Identity Role ie Admin
                new IdentityRole
                {
                    Name= "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                //Creating new Identity role for SuperAdmin
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                //creating new Identity role for User
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };
            //Now inserting all the above seed data in to buider object and using IdentityRole class and Has data
            builder.Entity<IdentityRole>().HasData(roles);

            //2.Seeding Superadmin User to give to Entity Framework
            var superAdminId = "8c4e67c3-da62-470a-adcd-73451788c438";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
                Id = superAdminId
            };
            //Before seeding Data in to DB, Generating password for Super Admin using PasswordHash class and HashPassword method
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            //Now inserting all the above seed data of SuperAdmin in to buider object and using IdentityUser class and Has data
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //3.Adding all roles like user, Admin and Superadmin to SuperAdminRole
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                //mapping 3 diff rolesusing  IdentityUser and Identity Role for same SuperAdmin Role
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };
            //Using builder object to seed the above data for SuperAdminRole
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}

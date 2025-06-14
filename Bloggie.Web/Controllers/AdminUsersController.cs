using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")] //Only Admins and SuperAdmins can view this page
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        //Using constructor we can inject IUserRepository to access the metod to get all users list
        public AdminUsersController(IUserRepository userRepository,UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        //Cretaing method to show all list of users from DB
        [HttpGet]
        public async Task<IActionResult> List()
        {
            //To fetch all the users of Auth Db via UserRepository
            var users = await userRepository.GetAll();

            //Now to display the users list inside a view by creating UserViewModel and User.cs view model
            //Initialising UserViewModel
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();
            //Iterating over users Domain model and showing the ID Username and Email on page
            foreach (var user in users)
            {

                //Inserting the data comming from Domain Model to View model
                usersViewModel.Users.Add(new Models.ViewModels.User
                {
                    //parsing string user ID to GUID
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email
                });
            }

            return View(usersViewModel);
        }
        //Now we need to capture the details from Get and post in to DB models using HttpPost method
        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)
        {
            //Creating Identity User object for IdentityUser first
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };
            var identityResult =
                await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    // If IdentityResult scceeded then assign roles to this user every user created will be assigned as user first by default
                    var roles = new List<string> { "User" };
                    //To give admin role only if the check box was selected means if it logins by admin credentials
                    if (request.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }

                    //again assigning roles to user
                    identityResult =
                        await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }

                }
            }
            //If the Identity result is null with no values in username pwd
            return View();
        }

        //Method to delete the User account  with input parameter as Id
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            //First we will find the user by ID and see if it exists in DB or not using userManager class
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is not null)
            {
                //If Id is not null then perform delete operation to remove user account and then redirect back to List page
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }
            //Else simply display the View page ie list page
            return View();
        }

    }

}

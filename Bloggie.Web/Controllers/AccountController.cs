using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        //Creating constructor for new service ie UserManager to create users 
        public AccountController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        //Displays Index page ie Register view page for collecting data
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        //This below action method is for collecting Register user info
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            
                //creating object of IdentityUser class as identityUser
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email
                };

                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                if (identityResult.Succeeded)
                {
                    // assign this user the "User" role
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                    if (roleIdentityResult.Succeeded)
                    {
                        // Show success notification and redirect to Register Action method
                        return RedirectToAction("Register");
                    }
                }
            

            // Show error notification
            return View();


        }
    }
}

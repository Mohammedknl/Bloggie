using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        //Creating constructor for new services ie UserManager to create users and signInManager to Login user 
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            //Below If condition is to check the Model state for server side validations
            if (ModelState.IsValid)
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

            }
            //If Model is valid it will register or else it will return View back

            // Show error notification
            return View();
        }
    //Below is for Login Functionality
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            //This is to redirect directly to particular page once the SuperAdmin logins
            //Creating instance or object of LoginViewModel and sending the retun url back to view page with a variable model
            var model = new LoginViewModel
            
            {
                ReturnUrl = ReturnUrl
            };

            //Instead of returning model we can simply return View 
            return View(model);
        }

        //To check the credentials for Login using signInManager
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username,
                loginViewModel.Password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
                //below if condition is to check if the return url was not null then can redirect to page of return url
                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
               
                //If sign in is successful then return back to Home page ie Index page of Home controller
                return RedirectToAction("Index", "Home");
            }

            // If user is not able to login return view back and Show errors
            return View();
        }

        //Logout Functionality Action method without any parameters and using signInManager injected classes
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //using signInManager we can logout the user using SignoutAsync
            await signInManager.SignOutAsync();
            //redirecting back to Index page of Home Controller
            return RedirectToAction("Index", "Home");
        }

        //Below is for Access denied page related to Authorization for other than SuperAdmin users
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

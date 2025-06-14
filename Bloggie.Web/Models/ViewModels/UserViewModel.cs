namespace Bloggie.Web.Models.ViewModels
{
    //This View model is used inside AdminUserController for displaying list of Users from Auth Db
    //Here we create one more view model to wrap all the users inside a list ie User.cs ViewModel
    public class UserViewModel
    {
        public List<User> Users { get; set; }

        //Below properties are to bind inside list view for User Modal class
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AdminRoleCheckbox { get; set; }
    }
}

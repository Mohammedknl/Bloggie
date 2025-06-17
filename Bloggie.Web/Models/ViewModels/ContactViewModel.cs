using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels
{
    //This class is for Contact page added new funcitonaity
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
namespace Bloggie.Web.Models.ViewModels
{
    public class AddTagRequest
    {
        //These properties will be used inside AdminTagsController.cs for mapping to domain models
        [Required]  //Server side validations
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}

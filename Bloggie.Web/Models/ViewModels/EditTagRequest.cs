namespace Bloggie.Web.Models.ViewModels
{
    //This is a EditTagRequest which takes a request
    public class EditTagRequest
    {

        public Guid Id { get; set; }  //Here Id will not be updated but required to read the Id field
        public string Name { get; set; }
        public string DisplayName { get; set; }

    }
}

using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        //Here we are using Repository methods inside controller to talk to DB insted of DbContext

        //Injecting TagRepository and BlogPostRepository
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }
        //S1.Creating Asynchronous Add method to add new Blogs it is also a Index or home page for Add New BlogPost page
        //In this Add functionality we are fetching only Tags to display on Multiple dropdown selection to display on Add views page
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // Talking to Repository to get tags from repository for displaying on multiple dropdown box inside add view
            var tags = await tagRepository.GetAllAsync();

            //after gettings tags from tagRepository we can assign tags to View Model AddBlogPostrequest
            //To fill Tags properties with Text as Name and value as Guid converted to string

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            //providing this model to View for Model Binding
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //Based on the Id's comming from AddBlogpostRequest we can assign the Tags to Blogpost as below
            
            //Mapping properties of View Model to Domain Model before we post the values
            //Blogpost is the Domain Model and addBlogPostRequest is the View Model

            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
            };

            // Maping Tags property from selected tags by looping through ID and get the values from DB
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            // Mapping tags back to domain model
            blogPost.Tags = selectedTags;


            await blogPostRepository.AddAsync(blogPost);



            return RedirectToAction("Add"); //Here we can also redirect to List method to display list of all Blogpost
        }
        //S2.Working on List Action Method to Display all Blogs by fetching from DB models
        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Calling the repository to get back all the Blogs data from DB using blogPostrepository Injected file
            var blogPosts = await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }

        //Now to perform Edit operation first we need to get the single blogpost back based on Id and
        //then display one form with all the fileds to edit the Blogpost
        //S1.For Edit first displaying a single blogpost based on ID using Edit Action method and HttpGet keyword
        [HttpGet]
        //asp-route-id="@blogPost.Id" thish should match with the parameter of Edit(Guid id) below
        public async Task<IActionResult> Edit(Guid id)
        {
            // Retrieve the result like single blog information along with tags from the repository using blogpostRepository and tagRepository
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                //mapping the domain model into the view model to show all fileds from DB model on web page for editing
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    //After getting all Tags we have to show it as a dropdown Selection list
                    //So converting tagsDomain in to dropdown select list using select Linq method
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        //Setting properties for object
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);

            }

            // pass data to view
            return View(null);
        }
        //S2 for Editing like once user fills the properties again it shoul update the DB models
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map view model back in to domain model to recieve the information
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible
            };

            // Mapping tags into domain model which is for navigation property

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            // Submit information to repository to update
            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                // Show success notification and redirect to Edit HttpGet action method
                return RedirectToAction("List");
            }

            // Show error notification
            return RedirectToAction("Edit");
        }

        //This action method is for deleting the BlogsPost
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            // Talk to repository to delete this blog post and tags
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            //
            if (deletedBlogPost != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }

            // Show error notification and redirect to same Edit page 
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });


        }
    }
}

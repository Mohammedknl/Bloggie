using System.Runtime.CompilerServices;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    //Here AdminTagsController is also a class file inherited from controller class of Asp.netCore.mvc
    //It is not a general class it is a Controller class which handles the users request 
    public class AdminTagsController : Controller
    {

        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }





        //1.Here IactionResult is the response type of this Action Method and name of the action method is Index/Add here which returns View
        //Bydefault IActionResult uses Get method with the Add Action method
        
        [HttpGet] //As this method is not having any Database connection no file reading and writing so no need of Asynchronous methods here 
        public IActionResult Add()  //with this we can route to https://localhost:xxx/AdminTags/Add instead of Index method
        {
            return View(); //Now to display view we have to create a new view page by name AdminTags/Add.cshtml inside views
        }

        //2.This is for creating/ saving entities in to Domain Model Tables and making the method Asynchronous
        [HttpPost]  
        [ActionName("Add")] //This is optional if we are mapping to Domain models
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            

            // Here Mapping AddTagRequest to Tag domain model to save Name and DisplayName fields inside DB Model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            //using await we don't need to wait for the other executions to finish
           await bloggieDbContext.Tags.AddAsync(tag); //making Database call and using await we make Asynchronous call

           await bloggieDbContext.SaveChangesAsync(); //writing in to DB

            // return View("Add");  After saving the data this will simply redirect to Add Index page of the controller
            //After saving if we want to redirect user to any other Action Methods we can do it using redirectAction method
            return RedirectToAction("List");   //for using redirection we have to declare the Action name before the action method like below
        }
        //3.Displaying all Tags in a tabular manner using Bootstrap table on web page to read records from Database models
        //Reading records from DB
        [HttpGet]  //Bydefault also it is HttpGet convention
        //we have to add new view by same Action method name List by rightclick add view on List
        [ActionName("List")]  //This is mandatory if we r using any RedirectToAction Methods like above
        public async Task<IActionResult> List()
        {
            //Using Db Context to read the Tags property of DbContext from DB
            //bloggieDbCOntext.Tags will connet to DBSet Tag of DOmain Model to fetch records
            var tags= await bloggieDbContext.Tags.ToListAsync(); //tags will have all list of Tags fetched from DB

            return View(tags);  //This way we do Model Binding to display all list of Tags
        }

        //4.Edit or updating and Deleting Records from DB Table
        [HttpGet]  //Annotating Get
        public async Task<IActionResult> Edit(Guid id)  //Here input parameter id should match with the route id in url
        {
            //using the route id we can read the record based on the id or url route id
            // Method 1 of finding the record using blogDbContext and route id 
            //var tag=bloggieDbContext.Tags.Find(id);
            //Method 2 to find the record based on Id and read inside the page using lambda expression and FirstOrDefault method
            var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);  //we can also single or defaultmethod to find records

            //Displaying this tag record on to Edit page for updating/Deleting using EditTagRequest
            if (tag != null)
            {
                //These variables can be used inside view to connect with Tag models
                var eidtTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName

                };
                return View(eidtTagRequest);


            }

            return View(null);


        }

        //5.Below is the New Post Method to Edit the records
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            //When we r reading the values using Model Binding we have EditTagRequest
            //Now we have to convert back this EditTagRequest to Tag Domain Model because the bloggieDbContext cares about only Entities and DOmain models not View models
            
            //So Mapping AddTagRequest View to Domain Model to perform update inside DB
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName

            };

            var existingTag=await bloggieDbContext.Tags.FindAsync(tag.Id); //this is the value comming from DB and it may or may not be null
            if (existingTag != null) 
            {
                //If Id is not null then we can now change/update the details as below
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                //we will save the changes to update the existing tag details comming from the request
                await bloggieDbContext.SaveChangesAsync();

                //Show Success Notification and return back to Edit page
                //return RedirectToAction("List"); we can return back to List or Edit page as below
                return RedirectToAction("Edit", new { id = editTagRequest.Id });

            
            }

            //Show an Error Notification
            return RedirectToAction("Edit",new { id=editTagRequest.Id});

        }

        //6.Creating Action method Delete to Delete the record with the HttpPost Anotation
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            //Here we need to check the Db if the entity exist or not
           var tag= bloggieDbContext.Tags.Find(editTagRequest.Id);
            if(tag!=null)
            {

                bloggieDbContext.Tags.Remove(tag);  //This will delete the record from Db Remove method has no Asynch keywords
                await bloggieDbContext.SaveChangesAsync();
                //Show Success Notification and return back to List page
                return RedirectToAction("List");
            }
            //If tag is null not found then return back to edit page
            //Show an error Notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }





    }
}

using System.Runtime.CompilerServices;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        //1.Here IactionResult is the response type of this Action Method and name of the action method is Index/Add here which returns View
        //Bydefault IActionResult uses Get method with the Add Action method
        
        [HttpGet] 
        public IActionResult Add()  
        {
            return View(); 
        }

        //2.This is for creating/ saving entities in to Domain Model Tables and making the method Asynchronous
        [HttpPost]  
        [ActionName("Add")] 
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {   
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);
            
            return RedirectToAction("List");   
        }
        //3.Displaying all Tags in a tabular manner using Bootstrap table on web page to read records from Database models
        //Reading records from DB
        [HttpGet]  
        
        [ActionName("List")]  
        public async Task<IActionResult> List()
        {
            
            var tags = await tagRepository.GetAllAsync(); 

            return View(tags);  
        }

        //4.Edit or updating and Deleting Records from DB Table
        [HttpGet]  //Annotating Get
        public async Task<IActionResult> Edit(Guid id)  
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        //5.Below is the New Post Method to Edit the records
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName

            };

            var updateTag = await tagRepository.UpdateAsync(tag);
            if (updateTag != null)
            {

                //Show success Notification
            }
            else 
            { 
            //Show error notification
            }
            return RedirectToAction("List");   //Or we can return back to List or Edit page as below
            //return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        //6.Creating Action method Delete to Delete the record with the HttpPost Anotation
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null)
            {
                // Show success notification and redirecting back to List page
                return RedirectToAction("List");
            }

            // Else Show an error notification and redirect back to Edit page
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }   
           
        }

    }


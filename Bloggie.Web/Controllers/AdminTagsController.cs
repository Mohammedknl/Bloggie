using System.Runtime.CompilerServices;
using Azure.Core;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")] //This is for Role base Authorization only for Admin
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
            //Below line is for Custom Validation Message
            ValidateAddTagRequest(addTagRequest);

            if (ModelState.IsValid == false)
            {
                return View();
            }
            // Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);
            
            return RedirectToAction("List");   
        }

        //This below method is responsible for Server side Custom Validation message for the Add tag Form if the Name and Display Name fields are same
        private void ValidateAddTagRequest(AddTagRequest request)
        {
            //Here we will add each and every property to add custome validation message
            if (request.Name is not null && request.DisplayName is not null)
            {
                if (request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be the same as DisplayName");
                }
            }
        }

        //3.Displaying all Tags in a tabular manner using Bootstrap table on web page to read records from Database models
        
        //Reading records from DB without sorting, filtering and Pagination
        //[HttpGet]

        //[ActionName("List")]
        //public async Task<IActionResult> List()
        //{

        //    var tags = await tagRepository.GetAllAsync();

        //    return View(tags);
        //}

        //New code to display List of Tags based on Searching and pagination
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageSize = 3, //this is for displaying no of elements/Tags result in each page here I changed from 3 to 1
            int pageNumber = 1)
        {
            //To get the count of Tags Tag Repository has
            //Based on the count on subset of pages we need to return the result
            var totalRecords = await tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            //If the page doesn't exit if no tags found on other pages then stick on same last page
            if (pageNumber > totalPages)
            {
                pageNumber--;
            }
            //If the page no is less then we can increase the page no to show all tags
            if (pageNumber < 1)
            {
                pageNumber++;
            }

            //This ViewBag is reposnsible for saving the input text to stay on the text field while searching
            ViewBag.TotalPages = totalPages;

            //To temporary save the state of searh Query we use ViewBag here and inside cshtml page list
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            // use dbContext to read the tags based on searchQuery sorting and pagination
            var tags = await tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

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


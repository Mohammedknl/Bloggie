using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

//This controller is for Ocntact Us page
namespace Bloggie.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Simulate a success (no saving or sending)
                TempData["Success"] = "Thank you for contacting us! We'll get back to you soon.";
                return RedirectToAction("Index");
            }

            return View(model); // Re-render with validation errors
        }
    }
}

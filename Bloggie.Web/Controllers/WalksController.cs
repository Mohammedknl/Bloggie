using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Services;

namespace Bloggie.Web.Controllers
{
    public class WalksController : Controller
    {
        private readonly IOMRCApiService _omrcApiService;
        private readonly ILogger<WalksController> _logger;

        public WalksController(IOMRCApiService omrcApiService, ILogger<WalksController> logger)
        {
            _omrcApiService = omrcApiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var walks = await _omrcApiService.GetWalksAsync();
            return View(walks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var walk = await _omrcApiService.GetWalkByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return View(walk);
        }
    }
}
using Bloggie.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{

    public class RegionsController : Controller
{
    private readonly IOMRCApiService _omrcApiService;
    private readonly ILogger<RegionsController> _logger;

    public RegionsController(IOMRCApiService omrcApiService, ILogger<RegionsController> logger)
    {
        _omrcApiService = omrcApiService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var regions = await _omrcApiService.GetRegionsAsync();
        return View(regions);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var region = await _omrcApiService.GetRegionByIdAsync(id);
        if (region == null)
        {
            return NotFound();
        }
        return View(region);
    }
}
}

using Bloggie.Web.Models; // Adjust namespace as per your project

namespace Bloggie.Web.Services
{
    public interface IOMRCApiService
    {
        Task<List<Region>> GetRegionsAsync();
        Task<List<Walk>> GetWalksAsync();
        Task<Region?> GetRegionByIdAsync(Guid id);
        Task<Walk?> GetWalkByIdAsync(Guid id);
    }
}
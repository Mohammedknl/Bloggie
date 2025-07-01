using System.Text.Json;
using Bloggie.Web.Models; // Adjust namespace as per your project

namespace Bloggie.Web.Services
{
    public class OMRCApiService : IOMRCApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OMRCApiService> _logger;
        private readonly string _baseUrl;

        public OMRCApiService(HttpClient httpClient, ILogger<OMRCApiService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["ApiSettings:OMRCApiBaseUrl"] ?? throw new InvalidOperationException("OMRCApiBaseUrl not configured");
        }

        public async Task<List<Region>> GetRegionsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching regions from OMRC API at {BaseUrl}", _baseUrl);
                var response = await _httpClient.GetAsync($"{_baseUrl}api/Regions");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var regions = JsonSerializer.Deserialize<List<Region>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("Successfully fetched {Count} regions", regions?.Count ?? 0);
                    return regions ?? new List<Region>();
                }
                else
                {
                    _logger.LogWarning("Failed to fetch regions. Status: {StatusCode}, URL: {Url}", response.StatusCode, $"{_baseUrl}api/Regions");
                    return new List<Region>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error while fetching regions from OMRC API at {BaseUrl}", _baseUrl);
                return new List<Region>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching regions from OMRC API");
                return new List<Region>();
            }
        }

        public async Task<List<Walk>> GetWalksAsync()
        {
            try
            {
                _logger.LogInformation("Fetching walks from OMRC API at {BaseUrl}", _baseUrl);
                var response = await _httpClient.GetAsync($"{_baseUrl}api/Walks");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var walks = JsonSerializer.Deserialize<List<Walk>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("Successfully fetched {Count} walks", walks?.Count ?? 0);
                    return walks ?? new List<Walk>();
                }
                else
                {
                    _logger.LogWarning("Failed to fetch walks. Status: {StatusCode}, URL: {Url}", response.StatusCode, $"{_baseUrl}api/Walks");
                    return new List<Walk>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error while fetching walks from OMRC API at {BaseUrl}", _baseUrl);
                return new List<Walk>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching walks from OMRC API");
                return new List<Walk>();
            }
        }

        public async Task<Region?> GetRegionByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching region {RegionId} from OMRC API at {BaseUrl}", id, _baseUrl);
                var response = await _httpClient.GetAsync($"{_baseUrl}api/Regions/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var region = JsonSerializer.Deserialize<Region>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("Successfully fetched region {RegionId}", id);
                    return region;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Region {RegionId} not found", id);
                    return null;
                }
                else
                {
                    _logger.LogWarning("Failed to fetch region {RegionId}. Status: {StatusCode}, URL: {Url}", id, response.StatusCode, $"{_baseUrl}api/Regions/{id}");
                    return null;
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error while fetching region {RegionId} from OMRC API", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching region {RegionId} from OMRC API", id);
                return null;
            }
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching walk {WalkId} from OMRC API at {BaseUrl}", id, _baseUrl);
                var response = await _httpClient.GetAsync($"{_baseUrl}api/Walks/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var walk = JsonSerializer.Deserialize<Walk>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("Successfully fetched walk {WalkId}", id);
                    return walk;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Walk {WalkId} not found", id);
                    return null;
                }
                else
                {
                    _logger.LogWarning("Failed to fetch walk {WalkId}. Status: {StatusCode}, URL: {Url}", id, response.StatusCode, $"{_baseUrl}api/Walks/{id}");
                    return null;
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error while fetching walk {WalkId} from OMRC API", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching walk {WalkId} from OMRC API", id);
                return null;
            }
        }
    }
}
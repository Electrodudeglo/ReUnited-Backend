using System.Net.Http.Headers;

namespace ReUnited_Backend.Services
{
    public class ImageStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ImageStorageService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
    }
}

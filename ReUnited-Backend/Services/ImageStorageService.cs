using System.Net.Http.Headers;

namespace ReUnited_Backend.Services
{
    public interface IImageStorageService
    {
        Task<string> UploadAsync(Stream stream, string originalFilename, string contentType);
    }
    public class ImageStorageService : IImageStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ImageStorageService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> UploadAsync(Stream stream, string originalFileName, string contentType)
        {
            var bucket =
                _configuration["Supabase:Bucket"];

            var supabaseUrl =
                _configuration["Supabase:Url"];

            var apiKey =
                _configuration["Supabase:ApiKey"];

            var fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";

            var storagePath =
                $"{bucket}/{fileName}";

            using var content =
                new StreamContent(stream);

            content.Headers.ContentType =
                new MediaTypeHeaderValue(contentType);

            using var request =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{supabaseUrl}/storage/v1/object/{storagePath}");

            request.Headers.Add("apikey", apiKey);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");

            request.Content = content;

            var response =
                await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return storagePath;
        }
    }
}

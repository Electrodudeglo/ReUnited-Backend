using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace ReUnited_Backend.Services
{
    public interface IImageStorageService
    {
        Task<string> UploadAsync(Stream stream, string originalFilename, string contentType);
    }
    public class ImageStorageService : IImageStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseSettings _settings;

        public ImageStorageService(HttpClient httpClient, IOptions<SupabaseSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<string> UploadAsync(Stream stream, string originalFileName, string contentType)
        {
            var bucket = _settings.Bucket;

            var supabaseUrl = _settings.Url;

            var apiKey = _settings.ApiKey;

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

using Microsoft.Extensions.Options;
using Supabase.Interfaces;
using System.Net.Http.Headers;

namespace ReUnited_Backend.Services
{
    public interface IImageStorageService
    {
        Task<string> UploadAsync(Stream stream, string originalFilename, string contentType);
        Task DeleteAsync(string storagePath);
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

        public async Task DeleteAsync(string storagePath)
        {
            var supabaseUrl = _settings.Url;

            var serviceRoleKey = _settings.ServiceRoleKey;

            using var request =
                new HttpRequestMessage(
                    HttpMethod.Delete,
                    $"{supabaseUrl}/storage/v1/object/{storagePath}");

            request.Headers.Add(
                "apikey",
                serviceRoleKey);

            request.Headers.Add(
                "Authorization",
                $"Bearer {serviceRoleKey}");

            var response =
                await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task<string> UploadAsync(Stream stream, string originalFileName, string contentType)
        {
            var bucket = _settings.Bucket;

            var supabaseUrl = _settings.Url;

            var serviceRoleKey = _settings.ServiceRoleKey;

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

            request.Headers.Add("apikey", serviceRoleKey);
            request.Headers.Add("Authorization", $"Bearer {serviceRoleKey}");

            request.Content = content;

            var response =
                await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return storagePath;
        }
    }
}

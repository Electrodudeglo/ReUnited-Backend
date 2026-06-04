using Microsoft.Extensions.Options;

namespace ReUnited_Backend.Services
{
    public class ImageUrlService
    {
        private readonly IConfiguration _configuration;

        public ImageUrlService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetPublicUrl(
            string storagePath)
        {
            var supabaseUrl =
                _configuration["Supabase:Url"];

            return
                $"{supabaseUrl}/storage/v1/object/public/{storagePath}";
        }
    }
}

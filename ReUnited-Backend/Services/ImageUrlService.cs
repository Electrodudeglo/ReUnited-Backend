using Microsoft.Extensions.Options;

namespace ReUnited_Backend.Services
{
    public class ImageUrlService
    {
        private readonly SupabaseSettings _settings;

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

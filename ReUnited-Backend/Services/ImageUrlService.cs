using Microsoft.Extensions.Options;

namespace ReUnited_Backend.Services
{
    public class ImageUrlService
    {
        private readonly SupabaseSettings _settings;

        public ImageUrlService(
            IOptions<SupabaseSettings> options)
        {
            _settings = options.Value;
        }

        public string GetPublicUrl(
            string storagePath)
        {
            return
                $"{_settings.Url}/storage/v1/object/public/{storagePath}";
        }
    }
}

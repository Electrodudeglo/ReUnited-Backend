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
    }
}

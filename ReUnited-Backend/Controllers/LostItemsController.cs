using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReUnited_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LostItemsController : ControllerBase
    {
        private readonly ILostItemService _lostItemService;

        public LostItemsController(ILostItemService lostItemService)
        {
            _lostItemService = lostItemService;
        }
    }
}

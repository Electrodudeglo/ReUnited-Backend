using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReUnited_Backend.Services;

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


        [HttpGet]
        public IActionResult GetLostItems() => Ok(_lostItemService.GetLostItems());

        [HttpGet]
        public IActionResult GetLostItemById(int id) => Ok(_lostItemService.GetLostItemsById(id));

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.DTOs;
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

        [HttpGet("{id}")]
        public IActionResult GetLostItemById(int id) => Ok(_lostItemService.GetLostItemsById(id));


        [HttpPost]
        public IActionResult AddOneLostItem(LostItem lostItem) => CreatedAtAction(nameof(GetLostItemById), new { id = lostItem.Id }, lostItem);
        

        [HttpPut("{id}")]
        public IActionResult UpdateLostItemById(UpdateLostItemDTO lostItem, int id)
        {
            var updatedLostItem = _lostItemService.UpdateLostItemById(lostItem, id);
            return Created($"/lostitems", updatedLostItem);
        }
    }
}

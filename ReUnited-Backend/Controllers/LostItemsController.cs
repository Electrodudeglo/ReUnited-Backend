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
        public IActionResult AddOneLostItem(LostItem lostItem)
        {
            LostItem addLostItem = _lostItemService.AddOneLostItem(lostItem);

            return Created("/lostitems", addLostItem);
        }
        

        [HttpPut("{id}")]
        public IActionResult UpdateLostItemById(UpdateLostItemDTO lostItem, int id)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var updatedLostItem = _lostItemService.UpdateLostItemById(lostItem, id);

                if (updatedLostItem == null) { return NotFound(); }

                return Ok(updatedLostItem);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the lost item.");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteLostItemById(int id)
        {
            var deletedLostItem = _lostItemService.DeleteLostItemById(id);

            if (!deletedLostItem)
            {
                return NotFound($"Lost item with ID {id} not found");
            }

            return NoContent();
        }
    }
}

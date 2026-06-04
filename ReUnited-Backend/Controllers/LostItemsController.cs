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
        private readonly IImageStorageService _imageStorageService;

        public LostItemsController(ILostItemService lostItemService, IImageStorageService imageStorageService)
        {
            _lostItemService = lostItemService;
            _imageStorageService = imageStorageService;
        }

        [HttpGet]
        public IActionResult GetLostItems() => Ok(_lostItemService.GetLostItems());

        [HttpGet("{id}")]
        public IActionResult GetLostItemById(int id) => Ok(_lostItemService.GetLostItemsById(id));


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddOneLostItem([FromForm] CreateLostItemDTO request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            
            if (request.Image is null)
            {
                return BadRequest("An image is required");
            }
            
            await using var stream = request.Image.OpenReadStream();

            var storagePath =
                await _imageStorageService.UploadAsync(
                stream,
                request.Image.FileName,
                request.Image.ContentType);

            var lostItem = new LostItem
            {
                City = request.City,
                Postcode = request.Postcode,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Category = request.Category,
                ItemDescription = request.ItemDescription,
                AdditionalInformation = request.AdditionalInformation,
                Picture = storagePath
            };

            LostItem addLostItem = _lostItemService.AddOneLostItem(lostItem);

            return CreatedAtAction(
                nameof(GetLostItemById),
                new { id = addLostItem.Id },
                addLostItem);
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

        //[Authorize]
        //[Authorize(Roles = "admin")]
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

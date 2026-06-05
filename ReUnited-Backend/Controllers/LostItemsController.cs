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
        private readonly ImageUrlService _imageUrlService;
        private static readonly string[] AllowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        private static readonly string[] AllowedContentTypes =
        {
            "image/jpeg",
            "image/png"
        };

        private const long MaxFileSize = 5 * 1024 * 1024;

        public LostItemsController(ILostItemService lostItemService, IImageStorageService imageStorageService, ImageUrlService imageUrlService)
        {
            _lostItemService = lostItemService;
            _imageStorageService = imageStorageService;
            _imageUrlService = imageUrlService;
        }

        [HttpGet]
        public IActionResult GetLostItems()
        {
            var items =
                _lostItemService
                    .GetLostItems()
                    .Select(item =>
                        new LostItemResponseDTO
                        {
                            Id = item.Id,
                            City = item.City,
                            Postcode = item.Postcode,
                            Email = item.Email,
                            PhoneNumber = item.PhoneNumber,
                            Category = item.Category,
                            ItemDescription = item.ItemDescription,
                            AdditionalInformation =
                                item.AdditionalInformation,
                            Picture =
                                _imageUrlService.GetPublicUrl(
                                    item.Picture)
                        });

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetLostItemById(int id)
        {
            var item =
                _lostItemService.GetLostItemsById(id);

            if (item == null)
            {
                return NotFound();
            }

            var dto = new LostItemResponseDTO
            {
                Id = item.Id,
                City = item.City,
                Postcode = item.Postcode,
                Email = item.Email,
                PhoneNumber = item.PhoneNumber,
                Category = item.Category,
                ItemDescription = item.ItemDescription,
                AdditionalInformation =
                    item.AdditionalInformation,
                Picture =
                    _imageUrlService.GetPublicUrl(
                        item.Picture)
            };

            return Ok(dto);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddOneLostItem([FromForm] CreateLostItemDTO request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (request.Image is null)
            {
                return BadRequest("An image is required");
            }

            var extension =
                Path.GetExtension(
                    request.Image.FileName)
                    .ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                return BadRequest(
                    "Only JPG, JPEG and PNG images are allowed.");
            }

            if (!AllowedContentTypes.Contains(request.Image.ContentType))
            {
                return BadRequest(
                    "Invalid image type.");
            }

            if (request.Image.Length > MaxFileSize)
            {
                return BadRequest(
                    "Maximum file size is 5MB.");
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

            var response = new LostItemResponseDTO
            {
                Id = addLostItem.Id,
                City = addLostItem.City,
                Postcode = addLostItem.Postcode,
                Email = addLostItem.Email,
                PhoneNumber = addLostItem.PhoneNumber,
                Category = addLostItem.Category,
                ItemDescription = addLostItem.ItemDescription,
                AdditionalInformation = addLostItem.AdditionalInformation,
                Picture = _imageUrlService.GetPublicUrl(addLostItem.Picture)
            };

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

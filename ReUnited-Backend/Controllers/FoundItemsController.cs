using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReUnited_Backend.DataModels;
using ReUnited_Backend.DTOs;
using ReUnited_Backend.Services;
using Supabase.Gotrue;
using System.Security.Claims;

namespace ReUnited_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoundItemsController : ControllerBase
    {
        private readonly IFoundItemService _foundItemService;
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

        public FoundItemsController(IFoundItemService foundItemService, IImageStorageService imageStorageService, ImageUrlService imageUrlService)
        {
            _foundItemService = foundItemService;
            _imageStorageService = imageStorageService;
            _imageUrlService = imageUrlService;
        }

        [HttpGet]
        public IActionResult GetFoundItems()
        {
            var items =
                _foundItemService
                    .GetFoundItems()
                    .Select(item =>
                        new FoundItemResponseDTO
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
                                    item.Picture),
                            UserId = item.UserId
                        });

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetFoundItemById(int id)
        {
            var item =
                _foundItemService.GetFoundItemsById(id);

            if (item == null)
            {
                return NotFound();
            }

            var dto = new FoundItemResponseDTO
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
                        item.Picture),
                UserId = item.UserId
            };

            return Ok(dto);
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddOneFoundItem([FromForm] CreateFoundItemDTO request)
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

            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var foundItem = new FoundItem
            {
                City = request.City,
                Postcode = request.Postcode,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Category = request.Category,
                ItemDescription = request.ItemDescription,
                AdditionalInformation = request.AdditionalInformation,
                Picture = storagePath,
                UserId = userId
            };

            FoundItem addFoundItem = _foundItemService.AddOneFoundItem(foundItem);

            var response = new FoundItemResponseDTO
            {
                Id = addFoundItem.Id,
                City = addFoundItem.City,
                Postcode = addFoundItem.Postcode,
                Email = addFoundItem.Email,
                PhoneNumber = addFoundItem.PhoneNumber,
                Category = addFoundItem.Category,
                ItemDescription = addFoundItem.ItemDescription,
                AdditionalInformation = addFoundItem.AdditionalInformation,
                Picture = _imageUrlService.GetPublicUrl(addFoundItem.Picture),
                UserId = addFoundItem.UserId
            };

            return CreatedAtAction(
                nameof(GetFoundItemById),
                new { id = addFoundItem.Id },
                addFoundItem);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateFoundItemById(UpdateFoundItemDTO foundItem, int id)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var userId =
                    User.FindFirstValue(ClaimTypes.NameIdentifier);

                var currentFoundItem = _foundItemService.GetFoundItemsById(id);

                if (currentFoundItem == null) { return NotFound(); }

                if (currentFoundItem.UserId != userId)
                {
                    return Forbid();
                }

                var updatedFoundItem = _foundItemService.UpdateFoundItemById(foundItem, id);

                return Ok(updatedFoundItem);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the found item.");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteFoundItemById(int id)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var currentFoundItem = _foundItemService.GetFoundItemsById(id);

            if (currentFoundItem == null) 
            { 
                return NotFound($"Found item with ID {id} not found"); 
            }

            if (currentFoundItem.UserId != userId)
            {
                return Forbid();
            }

            _foundItemService.DeleteFoundItemById(id);

            return NoContent();
        }

        [Authorize]
        [HttpGet("mine")]
        public IActionResult GetMyFoundItems()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var items =
                _foundItemService
                    .GetFoundItems()
                    .Where(item => item.UserId == userId)
                    .ToList();

            return Ok(items);
        }
    }
}

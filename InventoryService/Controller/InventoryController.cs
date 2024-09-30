using InventoryService.DbConfig;
using InventoryService.DTO;
using InventoryService.Models;
using InventoryService.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controller;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly InventoryDbContext _context;
    private readonly ProductService _productService;
    private readonly ImageService _imageService;

    // Constructor for Dependency Injection
    public InventoryController(InventoryDbContext context, ProductService _productService, ImageService imageService)
    {
        _context = context;
        _productService = _productService;
        _imageService = imageService;
    }
    
    // GET: api/Inventory
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryWithImagesDto>>> GetInventories()
    {
        var inventories = await _context.Inventories
            .Include(i => i.Images) // Include images for each inventory
            .ToListAsync();

        // Transforming the data into a DTO that contains full URLs for images
        var inventoryDtos = inventories.Select(inventory => new InventoryWithImagesDto
        {
            InventoryId = inventory.InventoryId,
            Name = inventory.Name,
            Price = inventory.Price,
            Description = inventory.Description,
            StockQuantity = inventory.StockQuantity,
            Images = inventory.Images.Select(image => new ImageDto
            {
                ImageId = image.ImageId,
                AltText = image.AltText,
                FileName = image.FileName,
                FileUrl = $"{Request.Scheme}://{Request.Host}/images/{image.FileName}"  // Full URL to access the image
            }).ToList()
        }).ToList();

        return Ok(inventoryDtos);
    }
    
    // GET: api/Inventory/5
    // [HttpGet("{id}")]
    // public async Task<ActionResult<Inventory>> GetInventory(int id)
    // {
    //     var inventory = await _context.Inventories.FindAsync(id);
    //
    //     if (inventory == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return inventory;
    // }
    
    // POST: api/Inventory
    [HttpPost]
    public async Task<ActionResult<Inventory>> PostInventory([FromForm] CreateInventoryDto createDto, [FromForm] List<IFormFile> files)
    {
        var inventory = new Inventory
        {
            Name = createDto.Name,
            Price = createDto.Price,
            Description = createDto.Description,
            StockQuantity = createDto.StockQuantity
        };
        
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
        
        if (files != null && files.Any())
        {
            await _imageService.UploadImagesAsync(inventory.InventoryId, files);
        }

        return CreatedAtAction(nameof(GetInventoryWithImages), new { id = inventory.InventoryId }, inventory);
    }
    
    // [HttpPost("sendProductId")]
    // public IActionResult SendExistingProductId(int productId)
    // {
    //     // Call the method to send productId to OrderService
    //     _productService.SendProductToOrderService(productId);
    //     return Ok($"Product ID {productId} sent to OrderService.");
    // }
    
    // PUT: api/Inventory/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInventory(int id, UpdateInventoryDto updateDto,  List<IFormFile> files)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory == null)
        {
            return NotFound();
        }

        inventory.Name = updateDto.Name;
        inventory.Price = updateDto.Price;
        inventory.Description = updateDto.Description;
        inventory.StockQuantity = updateDto.StockQuantity;

        _context.Entry(inventory).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InventoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        
        // If files are provided, upload new images and associate them with this inventory
        if (files != null && files.Any())
        {
            await _imageService.UploadImagesAsync(inventory.InventoryId, files);
        }


        return Ok(inventory);
    }
    
    // DELETE: api/Inventory/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventory(int id)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory == null)
        {
            return NotFound();
        }

        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();

        return Ok(inventory);
    }
    
    private bool InventoryExists(int id)
    {
        return _context.Inventories.Any(e => e.InventoryId == id);
    }
    

    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryWithImagesDto>> GetInventoryWithImages(int id)
    {
        var inventory = await _context.Inventories
            .Include(i => i.Images) // Include images
            .FirstOrDefaultAsync(i => i.InventoryId == id);

        if (inventory == null)
        {
            return NotFound();
        }

        // Transforming the data into a DTO that contains full URLs for images
        var inventoryDto = new InventoryWithImagesDto
        {
            InventoryId = inventory.InventoryId,
            Name = inventory.Name,
            Price = inventory.Price,
            Description = inventory.Description,
            StockQuantity = inventory.StockQuantity,
            Images = inventory.Images.Select(image => new ImageDto
            {
                ImageId = image.ImageId,
                AltText = image.AltText,
                FileName = image.FileName,
                FileUrl = $"{Request.Scheme}://{Request.Host}/images/{image.FileName}"  // Full URL to access the image
            }).ToList()
        };

        return Ok(inventoryDto);
    }

}
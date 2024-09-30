using InventoryService.DbConfig;
using InventoryService.Models;

namespace InventoryService.Services.Implementation;

public class ImageService
{
    private readonly InventoryDbContext _context;
    
    public ImageService(InventoryDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Image>> UploadImagesAsync(int inventoryId, List<IFormFile> files)
    {
        var imageList = new List<Image>();

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                // Generate a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Create a new image entry
                var image = new Image
                {
                    FileName = fileName,
                    FilePath = filePath,
                    InventoryId = inventoryId,
                    AltText = "Default Alt Text"
                };

                imageList.Add(image);
            }
        }

        // Save the images to the database
        if (imageList.Any())
        {
            await _context.Images.AddRangeAsync(imageList);
            await _context.SaveChangesAsync();
        }

        return imageList;
    }
}
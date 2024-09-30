namespace InventoryService.DTO;

public class ImageDto
{
    public int ImageId { get; set; }
    public string AltText { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }  // Full URL to the image
}
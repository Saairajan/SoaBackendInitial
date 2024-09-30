namespace InventoryService.DTO;

public class InventoryWithImagesDto
{
    public int InventoryId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int StockQuantity { get; set; }
    public List<ImageDto> Images { get; set; }
}
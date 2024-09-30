using System.ComponentModel.DataAnnotations;

namespace InventoryService.DTO;

public class UpdateInventoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public int StockQuantity { get; set; }
}
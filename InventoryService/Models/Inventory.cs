using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models;

public class Inventory
{
    [Key]
    public int InventoryId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public int StockQuantity { get; set; }
    
    public ICollection<Image> Images { get; set; }

}
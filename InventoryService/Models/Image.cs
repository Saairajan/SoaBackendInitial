using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models;

public class Image
{
    [Key]
    public int ImageId { get; set; }

    [Required]
    public string FileName { get; set; }

    [Required]
    public string FilePath { get; set; }

    public string AltText { get; set; }

    // Foreign key to Inventory
    public int InventoryId { get; set; }

    // Navigation property
    public Inventory Inventory { get; set; }

}
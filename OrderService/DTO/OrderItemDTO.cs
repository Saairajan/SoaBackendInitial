using System.ComponentModel.DataAnnotations;

namespace OrderService.DTO;

public class OrderItemDTO
{
    [Required]
    public int OrderItemId { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public decimal UnitPrice { get; set; }
}
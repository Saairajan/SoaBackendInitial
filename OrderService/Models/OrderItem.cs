using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    [Required]
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public decimal UnitPrice { get; set; }
}
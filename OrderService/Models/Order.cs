using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public string CustomerId { get; set; }

    public string ShippingAddress { get; set; }

    public List<OrderItem> OrderItems { get; set; }
}
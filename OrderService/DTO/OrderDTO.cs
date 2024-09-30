namespace OrderService.DTO;

public class OrderDTO
{
    public DateTime OrderDate { get; set; }
    
    public string CustomerId { get; set; }
    public string ShippingAddress { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
}
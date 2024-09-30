using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.DbConfig;
using OrderService.DTO;
using OrderService.Models;
namespace OrderService.Controller;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderDbContext _context;
    private readonly Services.Implementations.OrderService _orderService;
    
    public OrderController(OrderDbContext context, Services.Implementations.OrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }
    
    // GET: api/order
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
    }
    
    // GET: api/order/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders.Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null)
        {
            return NotFound();
        }
        return order;
    }
    
    // POST: api/order
    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder(OrderDTO orderDto)
    {
        // int productId = await _orderService.ReceiveProductDetails();
        var order = new Order
        {
            OrderDate = orderDto.OrderDate,
            CustomerId = orderDto.CustomerId,
            ShippingAddress = orderDto.ShippingAddress,
            OrderItems = orderDto.OrderItems.Select(item => new OrderItem
            {
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };  

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // orderDto.OrderId = order.OrderId; // Set the created OrderId
        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, orderDto);
    }
    
    // PUT: api/order/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, OrderDTO orderDto)
    {
        // int productId = await _orderService.ReceiveProductDetails();

        var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.OrderId == id);
        if (order == null)
        {
            return NotFound();
        }

        order.CustomerId = orderDto.CustomerId;
        order.ShippingAddress = orderDto.ShippingAddress;

        // Update OrderItems
        foreach (var itemDto in orderDto.OrderItems)
        {
            var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == itemDto.OrderItemId);
            if (orderItem != null)
            {
                orderItem.Quantity = itemDto.Quantity;
                orderItem.UnitPrice = itemDto.UnitPrice;
            }
            else
            {
                // Add new OrderItem if it doesn't exist
                order.OrderItems.Add(new OrderItem
                {
                    OrderId = itemDto.OrderId,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice
                });
            }
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    // DELETE: api/order/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }


}
using System.Text;
using Common.Services.Implementations;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InventoryService.Services.Implementation;

public class ProductService
{
    private readonly RabbitMqConfig _rabbitMqConfig;
    
    public ProductService(RabbitMqConfig rabbitMqConfig)
    {
        _rabbitMqConfig = rabbitMqConfig;
    }
    
    public void SendProductToOrderService(int productId)
    {
        // Create a message payload with ProductId
        var productMessage = new { ProductId = productId };
        var messageBody = JsonConvert.SerializeObject(productMessage);

        // Send ProductId to OrderService via RabbitMQ
        _rabbitMqConfig.PublishMessage("order_exchange" +  messageBody);
    }
}
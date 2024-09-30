using System.Text;
using Common.Services.Implementations;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace OrderService.Services.Implementations;

public class OrderService
{
    private readonly RabbitMqConfig _rabbitMqConfig;

    public OrderService(RabbitMqConfig rabbitMqConfig)
    {
        _rabbitMqConfig = rabbitMqConfig;
    }

     public async Task<int> ReceiveProductDetails()
     {
         var tcs = new TaskCompletionSource<int>();
     
         // Consume message from RabbitMQ queue
         _rabbitMqConfig.ConsumeMessage("product_queue", (message) => 
         {
             // Deserialize the message into a dynamic object
             var productMessage = JsonConvert.DeserializeObject<dynamic>(message);
     
             // Extract productId from the message
             int productId = productMessage.ProductId;
     
             // Set the result of the TaskCompletionSource
             tcs.SetResult(productId);
         });
     
         // Await until the productId is set
         return await tcs.Task;
     }
}
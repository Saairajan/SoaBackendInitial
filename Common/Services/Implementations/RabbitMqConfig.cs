using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Services.Implementations;

public class RabbitMqConfig
{
    public const string HostName = "localhost";
    public const string UserName = "guest";
    public const string Password = "guest";
    public const string QueueName = "productQueue";
    
    private IConnection _connection;
    private IModel _channel;
    
    public RabbitMqConfig()
    {
        CreateConnection();
    }
    
    private void CreateConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = HostName,
            UserName = UserName,
            Password = Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    
    public void PublishMessage(string messageBody)
    {
        if (_connection.IsOpen)
        {
            var body = Encoding.UTF8.GetBytes(messageBody);
            _channel.BasicPublish(exchange: "", // Use an exchange if needed
                routingKey: QueueName,
                basicProperties: null,
                body: body);
        }
        else
        {
            throw new InvalidOperationException("RabbitMQ connection is not open.");
        }
    }
    
    public void Dispose()
    {
        if (_channel != null && _channel.IsOpen)
        {
            _channel.Close();
        }
        if (_connection != null && _connection.IsOpen)
        {
            _connection.Close();
        }
    }
    
    public void ConsumeMessage(string queueName, Action<string> handleMessage)
    {
        var factory = new ConnectionFactory() { HostName = HostName, UserName = UserName, Password = Password };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handleMessage(message);  // Process the message
            };
            
            channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("Waiting for messages...");
            Console.ReadLine();  // Keeps the connection open to consume messages
        }
    }
    
}
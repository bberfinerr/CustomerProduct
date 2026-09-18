using RabbitMQ.Client;
using System.Text;

namespace CustomerProduct.Services
{
    public class RabbitMQService : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"],
                VirtualHost = configuration["RabbitMQ:VirtualHost"],
                Port = int.Parse(configuration["RabbitMQ:Port"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendMessage(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName,
                 durable: false,
                 exclusive: false,
                 autoDelete: false,
                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                 routingKey: queueName,
                 body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
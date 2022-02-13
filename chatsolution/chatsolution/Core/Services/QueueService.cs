using RabbitMQ.Client;
using System.Text;

namespace chatsolution.Core.Services
{
    public class QueueService : IQueueService
    {
        IConfiguration configuration;
        private readonly string serverAddress;

        public QueueService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.serverAddress = configuration.GetValue<string>("rabbitmq-address");
        }

        public void EnQueueMessage(TextMessage message)
        {
            var factory = new ConnectionFactory() { HostName = this.serverAddress };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "messages",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message.Text);

                channel.BasicPublish(exchange: "",
                                     routingKey: "messages",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Sent to RabbitMQ >> {message.Text}");
            }
        }
    }
}

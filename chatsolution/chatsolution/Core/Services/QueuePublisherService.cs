using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace chatsolution.Core.Services
{
    public class QueuePublisherService : IQueuePublisherService, IDisposable
    {
        IConfiguration configuration;
        private readonly string serverAddress;
        private readonly ILogger _logger;
        private IConnection? _connection;
        private IModel? _channel;

        public QueuePublisherService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.serverAddress = configuration.GetValue<string>("rabbitmq-address");
            this._logger = loggerFactory.CreateLogger<QueuePublisherService>();

            InitRabbitMQ();
        }

        public void EnQueueMessage(TextMessage message)
        {

                var body = Encoding.UTF8.GetBytes(message.Text);

                _channel.BasicPublish(exchange: "",
                                     routingKey: "messages",
                                     basicProperties: null,
                                     body: body);

                _logger.LogInformation($"Sent to RabbitMQ >> {message.Text}");
        }




        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = this.serverAddress };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("messages.exchange", ExchangeType.Topic);

            _channel.QueueDeclare(queue: "messages",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            _channel.QueueBind("messages", "messages.exchange", "messages.queue.*", null);
            _channel.BasicQos(0, 1, false);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}

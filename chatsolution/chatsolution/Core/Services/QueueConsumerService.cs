using chatsolution.Hubs;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace chatsolution.Core.Services
{
    public class QueueConsumerService : BackgroundService
    {
        IConfiguration configuration;
        private readonly string serverAddress;
        private readonly ILogger _logger;
        private IConnection? _connection;
        private IModel? _channel;
        IHubContext<ChatHub> chatHub;

        public QueueConsumerService(IConfiguration configuration, ILoggerFactory loggerFactory, IHubContext<ChatHub> chatHub)
        {
            this.configuration = configuration;
            this.serverAddress = configuration.GetValue<string>("rabbitmq-address");
            this._logger = loggerFactory.CreateLogger<QueuePublisherService>();
            this.chatHub = chatHub;

            InitRabbitMQ();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message  
                var content = System.Text.Encoding.UTF8.GetString(ea.Body.Span);

                // handle the received message  
                HandleMessage(content);
                _channel?.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("messages", false, consumer);
            return Task.CompletedTask;
        }

        private async void HandleMessage(string content)
        {
            _logger.LogInformation($"Queue received << {content}");

            await chatHub.Clients.All.SendAsync("ReceiveMessage", ChatBotDefinitions.UserName, content);
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


        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}

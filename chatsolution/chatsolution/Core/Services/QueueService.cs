using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace chatsolution.Core.Services
{
    public class QueueService : BackgroundService, IQueueService
    {
        IConfiguration configuration;
        private readonly string serverAddress;
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;

        public QueueService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.serverAddress = configuration.GetValue<string>("rabbitmq-address");
            this._logger = loggerFactory.CreateLogger<QueueService>();

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

            
            //_connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
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
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume("messages", false, consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(string content)
        {
            _logger.LogInformation($"Queue received << {content}");
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}

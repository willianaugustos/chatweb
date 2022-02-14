using chatsolution.Core;
using chatsolution.Core.Services;
using chatsolution.Data;
using Microsoft.AspNetCore.SignalR;

namespace chatsolution.Hubs
{
    public class ChatHub : Hub
    {
        IStockService stockService;
        IConfiguration configuration;
        IQueuePublisherService queueService;
        IMessageRepository messageRepository;
        private readonly ILogger _logger;

        //constructor: inject dependencies
        public ChatHub (IStockService StockService, IConfiguration Configuration, IQueuePublisherService queueservice, 
            IMessageRepository messageRepository,
            ILoggerFactory loggerFactory)
        {
            this.stockService = StockService;
            this.configuration = Configuration;
            this.queueService = queueservice;
            this.messageRepository = messageRepository;
            this._logger = loggerFactory.CreateLogger<QueuePublisherService>();
        }

        public async Task SendMessage(string username, string message)
        {

            //identify the message type
            var msg = MessageFactory.Create(username, message, DateTime.Now, stockService, configuration, queueService, _logger);

            //broadcast to all clients
            await BroadCastAllUsers(username, message);

            if (msg.ShouldSaveToDatabase())
            {
                //save to database
                await this.messageRepository.SaveAsync((TextMessage)msg);
            }

            if (msg is CommandMessage)
            {
                await ExecuteCommandMessage(msg);
            }

        }

        private async Task ExecuteCommandMessage(Message msg)
        {
            var commandMsg = ((CommandMessage)msg);

            //if the command is unrecognized, then feedback immediately
            if (commandMsg.IsUnKnownCommand())
            {
                await BroadCastAllUsers(ChatBotDefinitions.UserName, commandMsg.GetTextMessage());
            }
            else
            {
                //if is a recognized command, then execute:
                await commandMsg.DoWorkAsync();
            }
        }

        private async Task BroadCastAllUsers(string username, string message)
        {
            message = $"({DateTime.Now.ToString("HH:mm:ss")}) {message}";
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}

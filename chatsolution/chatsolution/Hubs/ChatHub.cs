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

        //constructor: inject dependencies
        public ChatHub (IStockService StockService, IConfiguration Configuration, IQueuePublisherService queueservice, IMessageRepository messageRepository)
        {
            this.stockService = StockService;
            this.configuration = Configuration;
            this.queueService = queueservice;
            this.messageRepository = messageRepository;
        }

        public async Task SendMessage(string username, string message)
        {

            //identify the message type
            var msg = MessageFactory.Create(username, message, stockService, configuration, queueService, messageRepository);

            //broadcast to all clients
            await BroadCastAllUsers(username, message);

            if (msg is TextMessage)
            {
                //save to database
                await ((TextMessage)msg).SaveToDatabaseAsync();
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
                var resultado = await commandMsg.DoWorkAsync();
            }
        }

        private async Task BroadCastAllUsers(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}

using chatsolution.Core;
using chatsolution.Core.Services;
using Microsoft.AspNetCore.SignalR;

namespace chatsolution.Hubs
{
    public class ChatHub : Hub
    {
        IStockService stockService;
        IConfiguration configuration;
        IQueuePublisherService queueService;

        //constructor: inject dependencies
        public ChatHub (IStockService StockService, IConfiguration Configuration, IQueuePublisherService queueservice)
        {
            this.stockService = StockService;
            this.configuration = Configuration;
            this.queueService = queueservice;
        }

        public async Task SendMessage(string username, string message)
        {

            //identify the message type
            var msg = MessageFactory.Create(username, message, stockService, configuration, queueService);

            //broadcast to all clients
            await BroadCastAllUsers(username, message);

            if (msg is TextMessage)
            {
                //save to database
                //await msg.SaveToDatabase();
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

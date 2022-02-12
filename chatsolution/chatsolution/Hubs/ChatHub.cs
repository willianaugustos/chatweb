using chatsolution.Core;
using chatsolution.Core.Services;
using Microsoft.AspNetCore.SignalR;

namespace chatsolution.Hubs
{
    public class ChatHub : Hub
    {
        IStockService stockService;

        //constructor: inject dependencies
        public ChatHub (IStockService StockService)
        {
            this.stockService = StockService;
        }

        public async Task SendMessage(string username, string message)
        {

            //identify the message type
            var msg = MessageFactory.Create(username, message, stockService);

            //broadcast to all clients
            await BroadCastAllUsers(username, message);

            if (msg is TextMessage)
            {
                //save to database
                //await msg.SaveToDatabase();
            }

            //
            if (msg is CommandMessage)
            {
                var commandMsg = ((CommandMessage)msg);

                if (commandMsg.IsUnKnownCommand())
                {
                    await BroadCastAllUsers(ChatBotDefinitions.UserName, commandMsg.GetTextMessage());
                }
                else
                {
                    //is a recognized command, so execute:
                    var resultado = await commandMsg.DoWorkAsync();
                }
            }

        }

        private async Task BroadCastAllUsers(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}

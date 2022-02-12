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

            if (msg is CommandMessage)
            {
                var msgCommand = ((CommandMessage)msg);

                if (msgCommand.IsUnKnownCommand())
                {
                    //BroadCastAllUsers(ChatBotDefinitions.UserName, msgCommand.GetTextMessage()[0]);
                }
                else
                {
                    //var task = msgCommand.DoWork();
                    //task.Wait();
                    //BroadCastAllUsers("test", task.Result);
                }
            }

        }

        private async Task BroadCastAllUsers(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}

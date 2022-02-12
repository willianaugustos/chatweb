using chatsolution.Core;
using Microsoft.AspNetCore.SignalR;

namespace chatsolution.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string username, string message)
        {

            //identify the message type
            var msg = MessageFactory.Create(username, message);
            
            //save to database
            if (msg is TextMessage)
            {
                //save
            }

            //broadcast to all clients
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}

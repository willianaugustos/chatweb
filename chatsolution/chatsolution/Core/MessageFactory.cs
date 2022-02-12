using chatsolution.Core.Services;

namespace chatsolution.Core
{
    public abstract class MessageFactory
    {

        public static Message Create(string username, string message, IStockService stockService)
        {
            Message msg;
            if (message.Trim().ToLower().StartsWith("/"))
            {
                msg = new CommandMessage(username, message, stockService);
            }
            else
            {
                msg = new TextMessage(username, message);
            }
            return msg;
        }

        
    }
}

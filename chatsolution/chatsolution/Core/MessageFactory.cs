using chatsolution.Core.Services;

namespace chatsolution.Core
{
    public abstract class MessageFactory
    {

        public static Message Create(string username, string message, IStockService stockService, IConfiguration configuration)
        {
            Message msg;
            if (message.Trim().ToLower().StartsWith("/"))
            {
                msg = new CommandMessage(username, message, stockService, configuration);
            }
            else
            {
                msg = new TextMessage(username, message);
            }
            return msg;
        }

        
    }
}

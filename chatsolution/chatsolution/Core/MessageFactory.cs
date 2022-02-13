using chatsolution.Core.Services;

namespace chatsolution.Core
{
    public abstract class MessageFactory
    {

        public static Message Create(string username, string message, IStockService stockService, IConfiguration configuration,
            IQueuePublisherService queueService)
        {
            Message msg;
            if (message.Trim().ToLower().StartsWith("/"))
            {
                msg = new CommandMessage(username, message, stockService, configuration, queueService);
            }
            else
            {
                msg = new TextMessage(username, message);
            }
            return msg;
        }

        
    }
}

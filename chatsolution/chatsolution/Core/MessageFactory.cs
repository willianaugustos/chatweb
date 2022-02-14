using chatsolution.Core.Services;
using chatsolution.Data;

namespace chatsolution.Core
{
    public abstract class MessageFactory
    {

        public static Message Create(string username, string message, DateTime dateTime, IStockService stockService, IConfiguration configuration,
            IQueuePublisherService queueService)
        {
            Message msg;
            if (message.Trim().ToLower().StartsWith("/"))
            {
                msg = new CommandMessage(username, message, dateTime, stockService, configuration, queueService);
            }
            else
            {
                msg = new TextMessage(username, message, dateTime);
            }
            return msg;
        }

        
    }
}

using chatsolution.Core.Services;
using chatsolution.Data;

namespace chatsolution.Core
{
    public abstract class MessageFactory
    {

        public static Message Create(string username, string message, IStockService stockService, IConfiguration configuration,
            IQueuePublisherService queueService,
            IMessageRepository messageRepository)
        {
            Message msg;
            if (message.Trim().ToLower().StartsWith("/"))
            {
                msg = new CommandMessage(username, message, stockService, configuration, queueService, messageRepository);
            }
            else
            {
                msg = new TextMessage(username, message, messageRepository);
            }
            return msg;
        }

        
    }
}

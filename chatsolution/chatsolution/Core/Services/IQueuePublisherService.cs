namespace chatsolution.Core.Services
{
    public interface IQueuePublisherService
    {
        void EnQueueMessage(Message message);
    }
}

namespace chatsolution.Core.Services
{
    public interface IQueueService
    {
        void EnQueueMessage(TextMessage message);
    }
}

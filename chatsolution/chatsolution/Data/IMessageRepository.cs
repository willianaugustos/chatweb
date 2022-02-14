using chatsolution.Core;

namespace chatsolution.Data
{
    public interface IMessageRepository
    {
        Task SaveAsync(TextMessage message);
    }
}

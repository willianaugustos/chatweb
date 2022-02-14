using chatsolution.Data;

namespace chatsolution.Core
{
    public class TextMessage : Message
    {
        private IMessageRepository messageRepository;
        public TextMessage(string from, string message, IMessageRepository messageRepository) : base(from, message)
        {
            this.messageRepository = messageRepository;
        }

        public async Task SaveToDatabaseAsync()
        {
            await this.messageRepository.SaveAsync(this);
        }
    }
}

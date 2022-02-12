namespace chatsolution.Core
{
    public enum MessageContentType
    {
        TextMessage = 0,
        CommandMessage = 1,
    }

    public abstract class Message
    {
        public string From { get; private set; }

        public Guid Id { get; private set; }

        public MessageContentType ContentType { get; private set; }

        public Message(string From, MessageContentType Type)
        {
            this.From = From;
            this.ContentType = Type;
        }
    }
}

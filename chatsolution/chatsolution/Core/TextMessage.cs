namespace chatsolution.Core
{
    public class TextMessage : Message
    {
        public string Text { get; private set; }

        public TextMessage(string From, string Text) : base(From, MessageContentType.TextMessage)
        {
            this.Text = Text;
        }
    }
}

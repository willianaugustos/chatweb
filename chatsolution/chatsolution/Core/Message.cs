using chatsolution.Data;

namespace chatsolution.Core
{
    public abstract class Message
    {

        public string From { get; private set; }
        public string Text{ get; private set; }

        public Message(string From, string Text)
        {
            this.From = From;
            this.Text = Text;
        }

        
    }
}

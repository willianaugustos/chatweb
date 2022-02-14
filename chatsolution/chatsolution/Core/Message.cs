using chatsolution.Data;

namespace chatsolution.Core
{
    public abstract class Message
    {
        public DateTime DateTime { get; private set; }
        public string From { get; private set; }
        public string Text{ get; private set; }

        public Message(string From, string Text, DateTime dateTime)
        {
            this.From = From;
            this.Text = Text;
            this.DateTime = dateTime;
        }

        
    }
}

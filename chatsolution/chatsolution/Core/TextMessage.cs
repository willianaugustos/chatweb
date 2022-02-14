using chatsolution.Data;

namespace chatsolution.Core
{
    public class TextMessage : Message
    {
        public TextMessage(string from, string message, DateTime dateTime) :
            base(from, message, dateTime)
        {
            
        }
    }
}

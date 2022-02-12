namespace chatsolution.Core
{
    public abstract class MessageFactory
    {
        

        public static Message Create(string username, string message)
        {
            Message msg;
            if (message.Trim().ToLower().StartsWith("/"))
            {
                msg = new CommandMessage(username, message);
            }
            else
            {
                msg = new TextMessage(username, message);
            }
            return msg;
        }

        
    }
}

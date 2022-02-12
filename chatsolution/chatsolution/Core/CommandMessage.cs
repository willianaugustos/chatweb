namespace chatsolution.Core
{
    public enum Command
    {
        UnknownCommand=0,
        StockQueryByCode=1
    }

    public class CommandMessage : Message
    {

        private const string stockCommand = "/stock=";
        private const string stockCodeArgument = "stock_code";

        public Command Command { get; private set; }
        public Dictionary<string, string> Arguments { get; private set; }

        public CommandMessage(string from, string message) : base(from, MessageContentType.CommandMessage)
        {
            this.Arguments = new Dictionary<string, string>();
            DetectAndfillArguments(message);
        }

        private void DetectAndfillArguments(string message)
        {
            if (message.Trim().ToLower().StartsWith(stockCommand))
            { 
                var stockCode = GetCommandMessageArgument(message);
                this.Arguments.Add(stockCodeArgument, stockCode);

                this.Command = Command.StockQueryByCode;
            }
            else
            {
                this.Command = Command.UnknownCommand;
            }
        }

        private string GetCommandMessageArgument(string message)
        {
            if (string.IsNullOrEmpty(message))
                return string.Empty;

            //separate message by = sign
            var arr = message.Split('=');
            if (arr.Length != 2)
            {
                return string.Empty;
            }

            //return the argument after = sign
            return arr[1];
        }
    }
}

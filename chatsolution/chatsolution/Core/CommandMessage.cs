using chatsolution.Core.Services;
using System.Text.RegularExpressions;

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
        private const string stockCommandSample = $"{stockCommand}{stockCodeArgument}";

        private IStockService StockService;

        private List<string> supportedCommands = new List<string>(){stockCommandSample};

        private string originalMessage { get; set; }
        public Command Command { get; private set; }
        public Dictionary<string, string> Arguments { get; private set; }

        public CommandMessage(string from, string message, IStockService StockService) : base(from, MessageContentType.CommandMessage)
        {
            this.StockService = StockService;
            this.Arguments = new Dictionary<string, string>();
            this.originalMessage = message;
            DetectAndfillArguments(message);
        }

        public string DoWork()
        {
            switch (this.Command)
            {
                case Command.StockQueryByCode:
                    string stock_code = this.Arguments[stockCodeArgument];
                    var result = this.StockService.QueryByCodeAsync(stock_code);
                    result.Wait();

                    
                    var value = GetStockPrice(result.Result);

                    return $"{stock_code} quote is {value} per share.";

                default:
                    break;
            }

            return String.Empty;
        }

        private string GetStockPrice(string result)
        {
            var regex = new Regex(@"\n(?:(?:[^,]+)\,){3}([^,]+)");
            var match = regex.Match(result);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns True when the Command property is of unknown
        /// </summary>
        /// <returns></returns>
        public bool IsUnKnownCommand()
        {
            return this.Command == Command.UnknownCommand;
        }

        /// <summary>
        /// Returns a formatted string containing a text message to be sent to the chat
        /// </summary>
        /// <returns></returns>
        public List<string> GetTextMessage()
        {
            if (this.Command == Command.UnknownCommand)
            {
                var supportedCommands = GetSupportedCommandsFormattedString();

                var result = new List<string>()
                { $"Command unknown ({this.originalMessage}). Try using one of the following supported commands:" };

                result.AddRange(GetSupportedCommandsFormattedString());

                return result;
            }
            else
            {
                return new List<string>() { "Command is in process, wait until it finishes." };
            }
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

        private List<string> GetSupportedCommandsFormattedString()
        {
            var formattedStringList = new List<string>();

            foreach (var item in this.supportedCommands)
            {
                formattedStringList.Add($"- {item}{Environment.NewLine}"); 
            }

            return formattedStringList;
        }

    }
}

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
            ParseArguments(message);
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <returns></returns>
        public async Task<string> DoWorkAsync()
        {
            switch (this.Command)
            {
                case Command.StockQueryByCode:
                    string stockCode = this.Arguments[stockCodeArgument];
                    var stockQueryResult = await this.StockService.QueryByCodeAsync(stockCode);
                    
                    var stockValue = GetStockPrice(stockQueryResult);
                    var info = GetStockInfoByValue(stockCode, stockValue);

                    return info;

                default:
                    break;
            }

            return String.Empty;
        }

        private string GetStockInfoByValue(string stockCode, string stockValue)
        {
            if (stockValue != "N/D")
            {
                return $"{stockCode} quote is {stockValue} per share.";
            }
            else
                return $"No information is available about this quote: {stockCode}";
        }

        private string GetStockPrice(string stockInfo)
        {
            var regex = new Regex(@"\n(?:(?:[^,]+)\,){3}([^,]+)");
            var match = regex.Match(stockInfo);

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
        public string GetTextMessage()
        {
            if (this.Command == Command.UnknownCommand)
            {
                var supportedCommands = GetSupportedCommandsFormattedString();
                
                return $"Command unknown ({this.originalMessage}). Try using the following supported command: {GetSupportedCommandsFormattedString()}";
            }
            else
            {
                return "Command is in process, wait until it finishes.";
            }
        }

        private void ParseArguments(string message)
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

        private string GetSupportedCommandsFormattedString()
        {
            var formattedStringList = "";

            foreach (var item in this.supportedCommands)
            {
                formattedStringList += $"{item} ";
            }

            return formattedStringList;
        }

    }
}

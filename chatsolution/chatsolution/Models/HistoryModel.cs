namespace chatsolution.Models
{
    public class HistoryModel
    {
        public string UserName { get; private set; }
        public string Message { get; private set; }

        public HistoryModel(string username, DateTime dateTime, string message)
        {
            this.UserName = username;
            this.Message = $"({dateTime.ToString("HH:mm:ss")}) {message}";
        }
    }
}

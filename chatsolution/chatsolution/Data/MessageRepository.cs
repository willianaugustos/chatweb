using chatsolution.Core;
using MySql.Data.MySqlClient;
using System.Data;

namespace chatsolution.Data
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<List<Message>> QueryLastMessages(int countLimit)
        {
            string sql = $"SELECT * FROM messages order by datetime desc limit {countLimit};";
            var dataTable = await base.ExecuteDataTableAsync(sql);
            return ConvertList(dataTable);
        }

        public async Task SaveAsync(TextMessage message)
        {
            string sql = "INSERT INTO messages (`datetime`, `from`, `message`) Values (@TimeStamp, @From, @Message);";

            await base.ExecuteNonQueryAsync(sql,
                new Parameter("@TimeStamp", DateTime.Now),
                new Parameter("@From", message.From),
                new Parameter("@Message", message.Text)
                ).ConfigureAwait(false);
        }

        private List<Message> ConvertList(DataTable dataTable)
        {
            var List = new List<Message>();
            foreach (DataRow row in dataTable.Rows)
            {
                List.Add(ConvertRecord(row));
            }
            return List;
        }

        private Message ConvertRecord(DataRow row)
        {
            return new TextMessage(base.ConvertToString(row["from"]),
                base.ConvertToString(row["message"]),
                base.ConvertToDateTime(row["datetime"]));
        }
    }
}

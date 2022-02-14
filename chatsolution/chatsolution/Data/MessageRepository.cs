using chatsolution.Core;
using MySql.Data.MySqlClient;
using System.Data;

namespace chatsolution.Data
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(IConfiguration configuration) : base(configuration)
        {

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
    }
}

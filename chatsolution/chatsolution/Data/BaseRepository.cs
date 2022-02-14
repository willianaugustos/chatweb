using MySql.Data.MySqlClient;

namespace chatsolution.Data
{
    public class Parameter
    {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public Parameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
    public abstract class BaseRepository
    {
        private MySqlConnection connection;
        private IConfiguration configuration;
        private readonly string connectionString;

        public BaseRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = configuration.GetValue<string>("db-connectionString");
            this.connection = new MySqlConnection(connectionString);
        }

        public async Task ExecuteNonQueryAsync(string sql, params Parameter[] parameters)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = sql;

            foreach (var param in parameters)
            {
                command.Parameters.Add(new MySqlParameter(param.Name, param.Value));
            }

            await command.ExecuteNonQueryAsync();
            connection.Close();
        }
    }
}

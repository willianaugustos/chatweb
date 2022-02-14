using MySql.Data.MySqlClient;
using System.Data;

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
    public abstract class BaseRepository<T>
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

        public async Task<DataTable> ExecuteDataTableAsync(string sql, params Parameter[] parameters)
        {
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = sql;

            foreach (var param in parameters)
            {
                command.Parameters.Add(new MySqlParameter(param.Name, param.Value));
            }

            var dataReader = await command.ExecuteReaderAsync();
            var dataTable = new DataTable();
            dataTable.Load(dataReader);

            connection.Close();

            return dataTable;
        }

        internal string ConvertToString(object value)
        {
            if (value.Equals(DBNull.Value) || value == null)
            {
                return String.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

        internal DateTime ConvertToDateTime(object value)
        {
            if (value.Equals(DBNull.Value) || value == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(value);
            }
        }
    }
}

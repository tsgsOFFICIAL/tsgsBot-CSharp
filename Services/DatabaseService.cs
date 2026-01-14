using Npgsql;

namespace tsgsBot_C_.Services
{
    public sealed class DatabaseService
    {
        private static readonly Lazy<DatabaseService> _instance = new(() => new DatabaseService());
        /// <summary>
        /// Gets the singleton instance of the database service.
        /// </summary>
        /// <remarks>This property provides global access to the shared database service instance. The
        /// instance is lazily initialized and is guaranteed to be thread-safe.</remarks>
        public static DatabaseService Instance => _instance.Value;
        
        private readonly DatabaseHelper _dbHelper;

        private DatabaseService()
        {
            _dbHelper = new DatabaseHelper();
        }
    }

    /// <summary>
    /// Provides asynchronous helper methods for executing SQL commands and queries against a PostgreSQL database.
    /// </summary>
    /// <remarks>This class simplifies common database operations such as executing non-query commands,
    /// retrieving data readers, and obtaining scalar values. All methods use the connection string specified within the
    /// class and are designed for use with PostgreSQL via the Npgsql library. The methods are intended for asynchronous
    /// usage and automatically manage database connections. This class is not thread-safe; create separate instances if
    /// used concurrently across multiple threads.</remarks>
    public class DatabaseHelper
    {
        private readonly string _connectionString = string.Empty;

        /// <summary>
        /// Initializes a new instance of the DatabaseHelper class using the connection string specified in the
        /// DB_CONNECTION_STRING environment variable.
        /// </summary>
        /// <remarks>Ensure that the DB_CONNECTION_STRING environment variable is configured before
        /// creating an instance of DatabaseHelper. This constructor is typically used to centralize database connection
        /// management based on environment configuration.</remarks>
        /// <exception cref="InvalidOperationException">Thrown if the DB_CONNECTION_STRING environment variable is not set or is empty.</exception>
        public DatabaseHelper()
        {
            _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("Connection string missing!");
        }

        /// <summary>
        /// Executes a SQL command that does not return any data, such as INSERT, UPDATE, or DELETE statements.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">An optional array of SQL parameters to include in the query.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteNonQueryAsync(string query, NpgsqlParameter[]? parameters = null)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        /// <summary>
        /// Executes a SQL query and returns a data reader to read the result set.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">An optional array of SQL parameters to include in the query.</param>
        /// <returns>A <see cref="NpgsqlDataReader"/> for reading the query results asynchronously.</returns>
        public async Task<NpgsqlDataReader> ExecuteReaderAsync(string query, NpgsqlParameter[]? parameters = null)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                return await command.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
            }
        }
        /// <summary>
        /// Executes a SQL query that returns a single value, such as a SELECT COUNT(*) query.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">An optional array of SQL parameters to include in the query.</param>
        /// <returns>The single value result of the query as an object.</returns>
        public async Task<object?> ExecuteScalarAsync(string query, NpgsqlParameter[]? parameters = null)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    return await command.ExecuteScalarAsync();
                }
            }
        }
    }
}
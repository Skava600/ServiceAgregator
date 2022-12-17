using Npgsql;

namespace ORM;

internal class DbConnector
{
    private readonly string connectionString;
    
    public DbConnector(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task ExecuteWithoutReaderAsync(string script, bool rootDb = false)
    {
        await using NpgsqlConnection connection = await this.GetConnectionAsync(rootDb);
        await using NpgsqlCommand command = new NpgsqlCommand(script, connection);
        await command.ExecuteNonQueryAsync();
    }
    
    public async Task<(NpgsqlConnection, NpgsqlDataReader)> ExecuteReaderAsync(string script, bool rootDb = false)
    {
        NpgsqlConnection connection = await this.GetConnectionAsync(rootDb);
        NpgsqlCommand command = new NpgsqlCommand(script, connection);
        return (connection, await command.ExecuteReaderAsync());
    }
    
    public async Task<object?> ExecuteScalarAsync(string script, bool rootDb = false)
    {
        await using NpgsqlConnection connection = await this.GetConnectionAsync(rootDb);
        await using NpgsqlCommand command = new NpgsqlCommand(script, connection);
        return await command.ExecuteScalarAsync();
    }

    private async Task<NpgsqlConnection> GetConnectionAsync(bool rootDb)
    {
        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(connectionString);
        
        if (rootDb)
        {
            builder.Database = "postgres";
        }
        
        NpgsqlConnection connection = new NpgsqlConnection(builder.ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}
using Npgsql;

namespace ORM;

internal class DbBasicScriptRunner
{
    private readonly DbConnector dbConnector;
    
    public DbBasicScriptRunner(string connectionString)
    {
        this.dbConnector = new DbConnector(connectionString);
    }

    public async Task<bool> DbExistsAsync(string dbName)
    {
        string script = DbScriptBuilder.DbExists(dbName);
        Console.WriteLine(script);
        return (await this.dbConnector.ExecuteScalarAsync(script, true) ?? "").ToString() == dbName;
    }
    
    public async Task CreateDbAsync(string dbName)
    {
        string script = DbScriptBuilder.CreateDb(dbName);
        Console.WriteLine(script);
        await this.dbConnector.ExecuteWithoutReaderAsync(script, true);
    }
    
    public async Task CreateTableIfNotExistsAsync(string tableName, IEnumerable<ColumnInfo> columns)
    {
        string script = DbScriptBuilder.CreateTableIfNotExists(tableName, columns);
        Console.WriteLine(script);
        await this.dbConnector.ExecuteWithoutReaderAsync(script);
    }   
    
    public async Task InsertValues(string tableName, Dictionary<string, string> values)
    {
        string script = DbScriptBuilder.InsertInto(tableName, values);
        Console.WriteLine(script);
        await this.dbConnector.ExecuteWithoutReaderAsync(script);
    }   
    
    public async Task UpdateValues(string tableName, string id, Dictionary<string, string> values)
    {
        string script = DbScriptBuilder.Update(tableName, id, values);
        Console.WriteLine(script);
        await this.dbConnector.ExecuteWithoutReaderAsync(script);
    }   
    
    public async Task<IEnumerable<Dictionary<string, string>>> Select(string tableName)
    {
        string script = DbScriptBuilder.SelectFrom(tableName);
        Console.WriteLine(script);
        var (connection, reader) = await this.dbConnector.ExecuteReaderAsync(script);
        var dictionary = await this.ReadAsDictionaryAsync(reader);
        await reader.DisposeAsync();
        await connection.DisposeAsync();

        return dictionary;
    }   
    
    public async Task<IEnumerable<Dictionary<string, string>>> SelectWhereField(string tableName, string field, string value)
    {
        string script = DbScriptBuilder.SelectFromWhereField(tableName, field, value);
        Console.WriteLine(script);
        var (connection, reader) = await this.dbConnector.ExecuteReaderAsync(script);
        var dictionary = await this.ReadAsDictionaryAsync(reader);
        await reader.DisposeAsync();
        await connection.DisposeAsync();

        return dictionary;
    }

    public async Task DeleteWhereField(string tableName, string field, string value)
    {
        string script = DbScriptBuilder.DeleteWhereField(tableName, field, value);
        Console.WriteLine(script);
        await this.dbConnector.ExecuteWithoutReaderAsync(script);
    }

    private async Task<IEnumerable<Dictionary<string, string>>> ReadAsDictionaryAsync(NpgsqlDataReader reader)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
        while (await reader.ReadAsync())
        {
            Dictionary<string, string> row = new Dictionary<string, string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Add(reader.GetName(i).ToUpper(), reader.GetValue(i).ToString());
            }
            result.Add(row);
        }

        return result;
    }
}
using System.Reflection;
using Npgsql;

namespace ORM;

public class DbContract
{
    private readonly DbBasicScriptRunner basicScriptRunner;
    private readonly string dbName;
    protected DbContract(string connectionString)
    {
        this.basicScriptRunner = new DbBasicScriptRunner(connectionString);
        this.dbName = new NpgsqlConnection(connectionString).Database;
        this.SetupTableInstances();
    }
    
    public async Task MigrateAsync()
    {
        if (!await this.basicScriptRunner.DbExistsAsync(this.dbName))
        {
            await this.basicScriptRunner.CreateDbAsync(this.dbName);
        }
        
        await this.MigrateTablesAsync();
    }

    internal async Task AddInstance(string name, Dictionary<string, string> values)
    {
        await this.basicScriptRunner.InsertValues(name, values);
    }
    
    internal async Task UpdateByIdAsync(string name, string id, Dictionary<string, string> values)
    {
        await this.basicScriptRunner.UpdateValues(name, id, values);
    }
    
    internal async Task<IEnumerable<Dictionary<string, string>>> ReadAll(string name)
    {
        return await this.basicScriptRunner.Select(name);
    }
    
    internal async Task<IEnumerable<Dictionary<string, string>>> GetAllByField(string name, string field, string value)
    {
        return await this.basicScriptRunner.SelectWhereField(name, field, value);
    }

    private void SetupTableInstances()
    {
        Type type = this.GetType();
        var properties = type.GetProperties()
            .Where(pi => pi.PropertyType.Name.StartsWith("DbTable"));

        foreach (var property in properties)
        {
            object instantiatedType =
                Activator.CreateInstance(property.PropertyType, BindingFlags.Public | BindingFlags.Instance,
                    null, new object[] { this, property.Name }, null)!;
            property.SetValue(this, instantiatedType);
        }
    }

    private async Task MigrateTablesAsync()
    {
        Type type = this.GetType();
        var properties = type.GetProperties()
            .Where(pi => pi.PropertyType.Name.StartsWith("DbTable"));

        foreach (var property in properties)
        {
            Type tableType = property.PropertyType.GetGenericArguments().First();
            await this.MigrateTableAsync(tableType, property.Name);
        }
    }

    private async Task MigrateTableAsync(Type type, string tableName)
    {
        var properties = type.GetProperties();
        List<ColumnInfo> columns = new List<ColumnInfo>();
        foreach (var property in properties)
        {
            if (!property.PropertyType.IsEnum)
            {
                string name = property.Name;
                string dbType = PropertyTypeMapper.MapType(property.PropertyType);
                IEnumerable<string> constraints = PropertyTypeMapper.MapConstraints(property);
                columns.Add(new ColumnInfo(name, dbType, constraints));
            }
            
        }

        await this.basicScriptRunner.CreateTableIfNotExistsAsync(tableName, columns);
    }
}
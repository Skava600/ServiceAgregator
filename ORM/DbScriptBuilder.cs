namespace ORM;

internal class DbScriptBuilder
{
    public static string DbExists(string dbName)
    {
        return $"SELECT datname FROM pg_catalog.pg_database WHERE datname = '{dbName}';";
    }
    
    public static string CreateDb(string dbName)
    {
        return $"CREATE DATABASE {dbName};";
    }
    
    public static string CreateTableIfNotExists(string tableName, IEnumerable<ColumnInfo> columns)
    {
        return $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", columns.Select(GetColumnInfo))});";
    }
    
    public static string InsertInto(string tableName, Dictionary<string, string> values)
    {
        string columnns = $"({string.Join(", ", values.Select(item => item.Key))})";
        string vals = $"({string.Join(", ", values.Where(item => item.Value != null).Select(item => $"'{item.Value}'"))})";
        return $"INSERT INTO public.{tableName}{columnns} VALUES{vals};";
    }

    public static string DeleteWhereField(string tableName, string field, string value)
    {
        return $"DELETE FROM {tableName} WHERE {field}='{value}';";
    }
    
    public static string Update(string tableName, string id, Dictionary<string, string> values)
    {
        string expr = string.Join(", ", values.Select(item => $"{item.Key}='{item.Value}'"));
        return $"UPDATE public.{tableName} SET {expr} WHERE id='{id}';";
    }
    
    public static string SelectFrom(string tableName)
    {
        return $"SELECT * FROM public.{tableName};";
    }
    
    public static string SelectFromWhereField(string tableName, string field, string value)
    {
        return $"SELECT * FROM public.{tableName} WHERE {field}='{value}';";
    }

    private static string GetColumnInfo(ColumnInfo columnInfo)
    {
        return $"{columnInfo.Name} {columnInfo.Type} {string.Join(' ', columnInfo.Constrains)}";
    }
}
namespace ORM;

internal static class TableMappingHelper
{
    public static Dictionary<string, string> MapToDictionary<T>(T instance)
    {
        var dictionary = new Dictionary<string, string>();
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(instance);
            if (value != null)
            {
                if (value.GetType() == typeof(DateTime))
                {
                    dictionary.Add(property.Name, ((DateTime)value).ToString("yyyy-MM-dd"));
                }
                else dictionary.Add(property.Name, value.ToString());    
            }
        }

        return dictionary;
    }

    public static T MapToInstance<T>(Dictionary<string, string> data)
        where T : new()
    {
        Type type = typeof(T);
        T instance = new T();
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            if (!property.PropertyType.IsEnum)
            {
                string value = data[property.Name.ToUpper()];
                object parsed = MapTypes(property.PropertyType, value);
                property.SetValue(instance, parsed);
            }
        }

        return instance;
    }

    private static object? MapTypes(Type type, string value)
    {
        if (type == typeof(int))
        {
            return int.Parse(value);
        }
        if (type == typeof(Guid))
        {
            return new Guid(value);
        }
        if (type == typeof(double))
        {
            return double.Parse(value);
        }
        if (type == typeof(double?))
        {
            return string.IsNullOrEmpty(value) ? null: double.Parse(value);
        }
        if (type == typeof(Guid?))
        {
            return string.IsNullOrEmpty(value) ? null : new Guid(value);
        }
        if (type == typeof(bool))
        {
            return bool.Parse(value);
        }
        if (type == typeof(DateTime))
        {
            return DateTime.Parse(value);
        }

        return value;
    }
}
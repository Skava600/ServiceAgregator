using System.Reflection;
using ORM.Attributes;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace ORM;

internal static class PropertyTypeMapper
{
    public static string MapType(Type type)
    {
        if (type == typeof(int))
        {
            return "integer";
        }
        if (type == typeof(float))
        {
            return "real";
        }
        else if (type == typeof(bool))
        {
            return "boolean";
        }
        else if (type == typeof(string))
        {
            return "text";
        }
        else if (type == typeof(Guid) || type == typeof(Guid?))
        {
            return "uuid";
        }
        else if (type == typeof(DateTime))
        {
            return "date";
        }
        else if (type == typeof(double) || type == typeof(double?))
        {
            return "double precision";
        }
        else if (type == typeof(short)) 
        {
            return "smallint";
        }

        throw new NotSupportedException();
    }
    
    public static IEnumerable<string> MapConstraints(PropertyInfo propertyInfo)
    {
        List<string> constraints = new List<string>();
        if (propertyInfo.GetCustomAttributes(typeof(NotNullAttribute)).Any())
        {
            constraints.Add("NOT NULL");
        }
        if (propertyInfo.GetCustomAttributes(typeof(PrimaryKeyAttribute)).Any())
        {
            constraints.Add("PRIMARY KEY");
        }
        if (propertyInfo.GetCustomAttributes(typeof(UniqueAttribute)).Any())
        {
            constraints.Add("UNIQUE");
        }

        return constraints;
    }
}
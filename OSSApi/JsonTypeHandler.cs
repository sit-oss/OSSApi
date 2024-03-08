using System.Data;
using Dapper;

namespace OSSApi;

internal class JsonTypeHandler: SqlMapper.ITypeHandler
{
    public void SetValue(IDbDataParameter parameter, object value)
    {
        parameter.Value = System.Text.Json.JsonSerializer.Serialize(value);
    }

    public object? Parse(Type destinationType, object value)
    {
        if (value is string s)
            return System.Text.Json.JsonSerializer.Deserialize(s, destinationType);
        return null;
    }
}
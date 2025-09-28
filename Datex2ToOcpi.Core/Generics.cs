using System.Reflection;

namespace Datex2ToOcpi.Core;

public static class Generics
{
    public static void CombineObjects(object target, object other)
    {
        var type = target.GetType();

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in properties)
        {
            if (!prop.CanRead || !prop.CanWrite)
                continue;

            var value = prop.GetValue(other);
            if (value != null)
            {
                prop.SetValue(target, value);
            }
        }

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var value = field.GetValue(other);
            if (value != null)
            {
                field.SetValue(target, value);
            }
        }
    }
}

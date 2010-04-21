using System;

namespace Loft
{
    public static class ReflectionExtensions
    {
        public static string GetValue(this Object obj, string name)
        {
            return obj.GetType().GetProperty(name).GetValue(obj, null) as string;
        }

        public static void SetValue(this Object obj, string name, string data)
        {
            obj.GetType().GetProperty(name).SetValue(obj, data, null); 
        }
    }
}
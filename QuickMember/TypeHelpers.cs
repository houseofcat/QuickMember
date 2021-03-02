using System;
using System.Linq;
using System.Reflection;

namespace QuickMember
{
    internal static class TypeHelpers
    {
        public static int Min(int x, int y)
        {
            return x < y ? x : y;
        }

        public static PropertyInfo[] GetTypeAndInterfaceProperties(this Type type, BindingFlags flags)
        {
            return !type.IsInterface
                ? type.GetProperties(flags)
                : (new[] { type })
                    .Concat(type.GetInterfaces())
                    .SelectMany(i => i.GetProperties(flags))
                    .ToArray();
        }
    }
}

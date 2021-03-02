using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;

namespace QuickMember.Extensions
{
    public static class ClassExtensions
    {
        private readonly static Type DescriptionType = typeof(DescriptionAttribute);
        private readonly static ConcurrentDictionary<(Type, string), string> _cachedDescriptionResults = new ConcurrentDictionary<(Type, string), string>();

        public static string GetPropertyDescription<T>(this T _, string propertyName)
        {
            var type = typeof(T);
            if (_cachedDescriptionResults.TryGetValue((type, propertyName), out var description))
            { return description; }
            else
            {
                var property = type.GetProperty(propertyName);
                var attrib = (DescriptionAttribute)property
                    ?.GetCustomAttributes(false)
                    .FirstOrDefault(attribute => attribute.GetType() == DescriptionType);
                _cachedDescriptionResults.TryAdd((type, propertyName), attrib?.Description);
                return attrib?.Description;
            }
        }

        public static string Description<T>(this T _) where T : class, new()
        {
            var type = typeof(T);
            if (_cachedDescriptionResults.TryGetValue((type, nameof(T)), out var description))
            { return description; }
            else
            {
                var attrib = (DescriptionAttribute)type
                    .GetCustomAttributes(false)
                    .FirstOrDefault(attribute => attribute.GetType() == DescriptionType);
                _cachedDescriptionResults.TryAdd((type, nameof(T)), attrib?.Description);
                return attrib?.Description;
            }
        }

        public static void Test<T>(this T input) where T : class, new()
        {
            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
        }
    }
}
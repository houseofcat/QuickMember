using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickMember.Extensions
{
    public static class QuickExtensions
    {
        private static string DefaultNameConversion(string input)
        {
            return input;
        }

        public static IEnumerable<string> GetPropertyList<T>(this T input, Func<string, string> nameConversion = default) where T : new()
        {
            if (nameConversion == null)
            { nameConversion = DefaultNameConversion; }

            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
            return objectMemberAccessor
                    .GetMembers()
                    .Select(x => nameConversion(x.Name));
        }

        public static IEnumerable<string> GetNonEnumerablePropertyList<T>(this T input, Func<string, string> nameConversion = default) where T : new()
        {
            if (nameConversion == null)
            { nameConversion = DefaultNameConversion; }

            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
            return objectMemberAccessor
                    .GetMembers()
                    .Where(x => !x.IsEnumerable)
                    .Select(x => nameConversion(x.Name));
        }

        public static Dictionary<string, object> GetNonEnumerableProperties<T>(this T input, List<string> exclusions, Func<string, string> nameConversion = default) where T : new()
        {
            if (nameConversion == null)
            { nameConversion = DefaultNameConversion; }

            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
            return objectMemberAccessor
                    .GetMembers()
                    .Where(x => !x.IsEnumerable)
                    .Where(x => !exclusions?.Contains(x.Name) ?? false)
                    .ToDictionary(x => nameConversion(x.Name), x => objectMemberAccessor[input, x.Name]);
        }

        public static Dictionary<string, object> GetProperties<T>(this T input, List<string> exclusions, Func<string, string> nameConversion = default) where T : new()
        {
            if (nameConversion == null)
            { nameConversion = DefaultNameConversion; }

            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
            return objectMemberAccessor
                    .GetMembers()
                    .Where(x => !exclusions?.Contains(x.Name) ?? false)
                    .ToDictionary(x => nameConversion(x.Name), x => objectMemberAccessor[input, x.Name]);
        }

        public static Dictionary<string, object> GetReadableProperties<T>(this T input, List<string> exclusions, Func<string, string> nameConversion = default) where T : new()
        {
            if (nameConversion == null)
            { nameConversion = DefaultNameConversion; }

            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
            return objectMemberAccessor
                    .GetMembers()
                    .Where(x => x.CanRead)
                    .Where(x => !exclusions?.Contains(x.Name) ?? false)
                    .ToDictionary(x => nameConversion(x.Name), x => objectMemberAccessor[input, x.Name]);
        }
    }
}

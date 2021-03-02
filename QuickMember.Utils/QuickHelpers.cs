using System.Collections.Generic;
using System.Linq;

namespace QuickMember.Utils
{
    public static class QuickHelpers
    {
        public static Dictionary<string, object> GetParameters<TIn>(this TIn input) where TIn : new()
        {
            var objectMemberAccessor = TypeAccessor.Create(typeof(TIn));
            return objectMemberAccessor
                    .GetMembers()
                    .ToDictionary(x => x.Name, x => objectMemberAccessor[input, x.Name]);
        }

        public static Dictionary<string, object> GetReadableParameters<TIn>(this TIn input) where TIn : new()
        {
            var objectMemberAccessor = TypeAccessor.Create(typeof(TIn));
            return objectMemberAccessor
                    .GetMembers()
                    .Where(x => x.CanRead)
                    .ToDictionary(x => x.Name, x => objectMemberAccessor[input, x.Name]);
        }
    }
}

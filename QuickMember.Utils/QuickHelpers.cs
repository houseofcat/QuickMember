using System.Collections.Generic;
using System.Linq;

namespace QuickMember.Utils
{
    public class QuickHelpers
    {
        public Dictionary<string, object> GetParameters<TIn>(TIn input) where TIn : new()
        {
            var objectMemberAccessor = TypeAccessor.Create(input.GetType());

            return objectMemberAccessor
                    .GetMembers()
                    .ToDictionary(x => x.Name, x => objectMemberAccessor[input, x.Name]);
        }

        public Dictionary<string, object> GetReadableParameters<TIn>(TIn input) where TIn : new()
        {
            var objectMemberAccessor = TypeAccessor.Create(input.GetType());
            var propertyDictionary = new Dictionary<string, object>();
            foreach (var member in objectMemberAccessor.GetMembers())
            {
                if (member.CanRead)
                { propertyDictionary.Add(member.Name, objectMemberAccessor[input, member.Name]); }
            }

            return propertyDictionary;
        }
    }
}

using System;
using System.ComponentModel;
using System.Linq;

namespace QuickMember.Extensions
{
    public static class EnumExtensions
    {
        private readonly static Type DescriptionType = typeof(DescriptionAttribute);

        public static string Description(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            var attrib = (DescriptionAttribute)field
                ?.GetCustomAttributes(false)
                .FirstOrDefault(attribute => attribute.GetType() == DescriptionType);

            return attrib?.Description;
        }
    }
}
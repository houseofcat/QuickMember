using System;
using System.Linq;
using System.Reflection;

namespace QuickMember
{
    public sealed class Member
    {
        private readonly MemberInfo _member;

        internal Member(MemberInfo member)
        {
            _member = member;
        }

        public int Ordinal
        {
            get
            {
                var ordinalAttr = _member
                    .CustomAttributes
                    .FirstOrDefault(p => p.AttributeType == typeof(OrdinalAttribute));

                if (ordinalAttr == null)
                {
                    return -1;
                }

                // OrdinalAttribute class must have only one constructor with a single argument.
                return Convert.ToInt32(ordinalAttr.ConstructorArguments.Single().Value);
            }
        }

        public string Name { get { return _member.Name; } }

        public Type Type
        {
            get
            {
                if (_member is FieldInfo fieldInfo) return fieldInfo.FieldType;
                if (_member is PropertyInfo propertyInfo) return propertyInfo.PropertyType;
                throw new NotSupportedException(_member.GetType().Name);
            }
        }

        public bool IsDefined(Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return Attribute.IsDefined(_member, attributeType);
        }

        public Attribute GetAttribute(Type attributeType, bool inherit)
            => Attribute.GetCustomAttribute(_member, attributeType, inherit);


        public bool CanWrite
        {
            get
            {
                return _member.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)_member).CanWrite,
                    _ => throw new NotSupportedException(_member.MemberType.ToString()),
                };
            }
        }

        public bool CanRead
        {
            get
            {
                return _member.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)_member).CanRead,
                    _ => throw new NotSupportedException(_member.MemberType.ToString()),
                };
            }
        }
    }
}

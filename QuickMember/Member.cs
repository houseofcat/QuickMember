using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace QuickMember
{
    public sealed class Member
    {
        public readonly MemberInfo MemberInfo;

        internal Member(MemberInfo member)
        {
            MemberInfo = member;
        }

        public string Name { get { return MemberInfo.Name; } }

        public Type Type
        {
            get
            {
                if (MemberInfo is FieldInfo fieldInfo) return fieldInfo.FieldType;
                if (MemberInfo is PropertyInfo propertyInfo) return propertyInfo.PropertyType;
                throw new NotSupportedException(MemberInfo.GetType().Name);
            }
        }

        public bool IsValueType
        {
            get
            {
                return Type.IsValueType;
            }
        }

        public bool IsClass
        {
            get
            {
                return Type.IsClass;
            }
        }

        public bool IsEnum
        {
            get
            {
                return Type.IsEnum;
            }
        }

        public bool IsString
        {
            get
            {
                return Type == typeof(string);
            }
        }

        public bool IsEnumerable
        {
            get
            {
                return Type != typeof(string)
                    && typeof(IEnumerable).IsAssignableFrom(Type);
            }
        }

        public int Ordinal
        {
            get
            {
                var ordinalAttr = MemberInfo
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

        public bool CanWrite
        {
            get
            {
                return MemberInfo.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)MemberInfo).CanWrite,
                    _ => throw new NotSupportedException(MemberInfo.MemberType.ToString()),
                };
            }
        }

        public bool CanRead
        {
            get
            {
                return MemberInfo.MemberType switch
                {
                    MemberTypes.Property => ((PropertyInfo)MemberInfo).CanRead,
                    _ => throw new NotSupportedException(MemberInfo.MemberType.ToString()),
                };
            }
        }

        public bool IsNestedClass
        {
            get
            {
                return MemberInfo.MemberType switch
                {
                    MemberTypes.NestedType => true,
                    _ => throw new NotSupportedException(MemberInfo.MemberType.ToString()),
                };
            }
        }

        public bool IsSubclass(Type parentType)
        {
            return Type.IsSubclassOf(parentType);
        }

        public bool IsDefined(Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return Attribute.IsDefined(MemberInfo, attributeType);
        }

        public Attribute GetAttribute(Type attributeType, bool inherit)
            => Attribute.GetCustomAttribute(MemberInfo, attributeType, inherit);
    }
}

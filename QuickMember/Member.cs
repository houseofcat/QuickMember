using System;
using System.Linq;
using System.Reflection;

namespace QuickMember
{
    public sealed class Member
    {
        private readonly MemberInfo member;
        internal Member(MemberInfo member)
        {
            this.member = member;
        }
        /// <summary>
        /// The ordinal of this member among other members.
        /// Returns -1 in case the ordinal is not set.
        /// </summary>
        public int Ordinal
        {
            get
            {
                var ordinalAttr = member.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(OrdinalAttribute));

                if (ordinalAttr == null)
                {
                    return -1;
                }

                // OrdinalAttribute class must have only one constructor with a single argument.
                return Convert.ToInt32(ordinalAttr.ConstructorArguments.Single().Value);
            }
        }
        /// <summary>
        /// The name of this member
        /// </summary>
        public string Name { get { return member.Name; } }
        /// <summary>
        /// The type of value stored in this member
        /// </summary>
        public Type Type
        {
            get
            {
                if (member is FieldInfo) return ((FieldInfo)member).FieldType;
                if (member is PropertyInfo) return ((PropertyInfo)member).PropertyType;
                throw new NotSupportedException(member.GetType().Name);
            }
        }

        /// <summary>
        /// Is the attribute specified defined on this type
        /// </summary>
        public bool IsDefined(Type attributeType)
        {
            if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));
            return Attribute.IsDefined(member, attributeType);
        }

        /// <summary>
        /// Getting Attribute Type
        /// </summary>
        public Attribute GetAttribute(Type attributeType, bool inherit)
            => Attribute.GetCustomAttribute(member, attributeType, inherit);

        /// <summary>
        /// Property Can Write
        /// </summary>
        public bool CanWrite
        {
            get
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Property: return ((PropertyInfo)member).CanWrite;
                    default: throw new NotSupportedException(member.MemberType.ToString());
                }
            }
        }

        /// <summary>
        /// Property Can Read
        /// </summary>
        public bool CanRead
        {
            get
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Property: return ((PropertyInfo)member).CanRead;
                    default: throw new NotSupportedException(member.MemberType.ToString());
                }
            }
        }
    }
}

using System;
using System.Dynamic;

namespace QuickMember
{
    public abstract class ObjectAccessor
    {
        public abstract object this[string name] { get; set; }
        public abstract object Target { get; }

        public override bool Equals(object obj)
        {
            return Target.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Target.GetHashCode();
        }

        public override string ToString()
        {
            return Target.ToString();
        }

        public static ObjectAccessor Create(object target)
        {
            return Create(target, false);
        }

        public static ObjectAccessor Create(object target, bool allowNonPublicAccessors)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            // Old Way
            // IDynamicMetaObjectProvider dlr = target as IDynamicMetaObjectProvider;
            // if (dlr != null) return new DynamicWrapper(dlr); // use the DLR

            if (target is IDynamicMetaObjectProvider dlr) return new DynamicWrapper(dlr);

            return new TypeAccessorWrapper(target, TypeAccessor.Create(target.GetType(), allowNonPublicAccessors));
        }
    }
}

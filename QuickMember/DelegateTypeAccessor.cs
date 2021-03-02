using System;
using System.Collections.Generic;

namespace QuickMember
{
    sealed class DelegateTypeAccessor : RuntimeTypeAccessor
    {
        private readonly Dictionary<string, int> _map;
        private readonly Func<int, object, object> _getter;
        private readonly Action<int, object, object> _setter;
        private readonly Func<object> _ctor;
        private readonly Type _type;

        protected override Type Type
        {
            get { return _type; }
        }

        public DelegateTypeAccessor(
            Dictionary<string, int> map,
            Func<int, object, object> getter,
            Action<int, object, object> setter,
            Func<object> ctor, Type type)
        {
            _map = map;
            _getter = getter;
            _setter = setter;
            _ctor = ctor;
            _type = type;
        }

        public override bool CreateNewSupported { get { return _ctor != null; } }

        public override object CreateNew()
        {
            return _ctor != null ? _ctor() : base.CreateNew();
        }

        public override object this[object target, string name]
        {
            get
            {
                if (_map.TryGetValue(name, out int index)) return _getter(index, target);

                else throw new ArgumentOutOfRangeException(nameof(name));
            }
            set
            {
                if (_map.TryGetValue(name, out int index)) _setter(index, target, value);
                else throw new ArgumentOutOfRangeException(nameof(name));
            }
        }
    }
}

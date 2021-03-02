using System.Dynamic;

namespace QuickMember
{
    sealed class DynamicWrapper : ObjectAccessor
    {
        private readonly IDynamicMetaObjectProvider _target;

        public override object Target
        {
            get { return _target; }
        }

        public DynamicWrapper(IDynamicMetaObjectProvider target)
        {
            _target = target;
        }

        public override object this[string name]
        {
            get { return CallSiteCache.GetValue(name, _target); }
            set { CallSiteCache.SetValue(name, _target, value); }
        }
    }
}

namespace QuickMember
{
    sealed class TypeAccessorWrapper : ObjectAccessor
    {
        private readonly object _target;
        private readonly TypeAccessor _accessor;

        public TypeAccessorWrapper(object target, TypeAccessor accessor)
        {
            _target = target;
            _accessor = accessor;
        }

        public override object this[string name]
        {
            get { return _accessor[_target, name]; }
            set { _accessor[_target, name] = value; }
        }

        public override object Target
        {
            get { return _target; }
        }
    }
}

namespace QuickMember
{
    sealed class DynamicAccessor : TypeAccessor
    {
        public static readonly DynamicAccessor Singleton = new DynamicAccessor();

        private DynamicAccessor() { }

        public override object this[object target, string name]
        {
            get { return CallSiteCache.GetValue(name, target); }
            set { CallSiteCache.SetValue(name, target, value); }
        }
    }
}

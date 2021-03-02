using System;

namespace QuickMember
{
    public abstract class RuntimeTypeAccessor : TypeAccessor
    {
        protected abstract Type Type { get; }

        public override bool GetMembersSupported { get { return true; } }

        private MemberSet _members;

        public override MemberSet GetMembers()
        {
            return _members ??= new MemberSet(Type);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickMember
{
    public sealed class MemberSet : IEnumerable<Member>, IList<Member>
    {
        private Member[] _members;

        internal MemberSet(Type type)
        {
            const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;

            _members = type
                .GetTypeAndInterfaceProperties(PublicInstance)
                .Cast<MemberInfo>()
                .Concat(type.GetFields(PublicInstance)
                .Cast<MemberInfo>())
                .OrderBy(x => x.Name)
                .Select(member => new Member(member))
                .ToArray();
        }

        public IEnumerator<Member> GetEnumerator()
        {
            foreach (var member in _members) yield return member;
        }

        public Member this[int index]
        {
            get { return _members[index]; }
        }

        public int Count { get { return _members.Length; } }

        Member IList<Member>.this[int index]
        {
            get { return _members[index]; }
            set { throw new NotSupportedException(); }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }

        bool ICollection<Member>.Remove(Member item) { throw new NotSupportedException(); }
        void ICollection<Member>.Add(Member item) { throw new NotSupportedException(); }
        void ICollection<Member>.Clear() { throw new NotSupportedException(); }
        void IList<Member>.RemoveAt(int index) { throw new NotSupportedException(); }
        void IList<Member>.Insert(int index, Member item) { throw new NotSupportedException(); }

        bool ICollection<Member>.Contains(Member item)  => _members.Contains(item);
        void ICollection<Member>.CopyTo(Member[] array, int arrayIndex) { _members.CopyTo(array, arrayIndex); }
        bool ICollection<Member>.IsReadOnly { get { return true; } }
        int IList<Member>.IndexOf(Member member) { return Array.IndexOf(_members, member); }
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace QuickMember
{
    public class ObjectReader : DbDataReader
    {
        private IEnumerator _source;
        private readonly TypeAccessor _accessor;
        private readonly string[] _memberNames;
        private readonly Type[] _effectiveTypes;
        private readonly BitArray _allowNull;

        private object _current;
        private bool _active = true;

        public ObjectReader(Type type, IEnumerable source, params string[] members)
        {
            if (source == null) throw new ArgumentOutOfRangeException(nameof(source));

            var noMemberParams = members == null || members.Length == 0;

            _accessor = TypeAccessor.Create(type);
            if (_accessor.GetMembersSupported)
            {
                // Sort members by ordinal first and then by name.
                var typeMembers = _accessor
                    .GetMembers()
                    .OrderBy(p => p.Ordinal)
                    .ToList();

                if (noMemberParams)
                {
                    members = new string[typeMembers.Count];
                    for (int i = 0; i < members.Length; i++)
                    {
                        members[i] = typeMembers[i].Name;
                    }
                }

                _allowNull = new BitArray(members.Length);
                _effectiveTypes = new Type[members.Length];
                for (int i = 0; i < members.Length; i++)
                {
                    Type memberType = null;
                    var allowNull = true;
                    var hunt = members[i];

                    foreach (var member in typeMembers)
                    {
                        if (member.Name == hunt)
                        {
                            if (memberType == null)
                            {
                                var tmp = member.Type;
                                memberType = Nullable.GetUnderlyingType(tmp) ?? tmp;

                                allowNull = !(memberType.IsValueType && memberType == tmp);

                                // but keep checking, in case of duplicates
                            }
                            else
                            {
                                memberType = null; // duplicate found; say nothing
                                break;
                            }
                        }
                    }

                    _allowNull[i] = allowNull;
                    _effectiveTypes[i] = memberType ?? typeof(object);
                }
            }
            else if (noMemberParams)
            {
                throw new InvalidOperationException("Member information is not available for this type; the required members must be specified explicitly");
            }

            _current = null;
            _memberNames = (string[])members.Clone();
            _source = source.GetEnumerator();
        }

        public static ObjectReader Create<T>(IEnumerable<T> source, params string[] members)
        {
            return new ObjectReader(typeof(T), source, members);
        }

        public override int Depth
        {
            get { return 0; }
        }

        public override DataTable GetSchemaTable()
        {
            // these are the columns used by DataTable load
            var table = new DataTable
            {
                Columns =
                {
                    {"ColumnOrdinal", typeof(int)},
                    {"ColumnName", typeof(string)},
                    {"DataType", typeof(Type)},
                    {"ColumnSize", typeof(int)},
                    {"AllowDBNull", typeof(bool)}
                }
            };

            var rowData = new object[5];
            for (int i = 0; i < _memberNames.Length; i++)
            {
                rowData[0] = i;
                rowData[1] = _memberNames[i];
                rowData[2] = _effectiveTypes == null ? typeof(object) : _effectiveTypes[i];
                rowData[3] = -1;
                rowData[4] = _allowNull == null ? true : _allowNull[i];
                table.Rows.Add(rowData);
            }

            return table;
        }

        public override void Close()
        {
            Shutdown();
        }

        public override bool HasRows
        {
            get { return _active; }
        }

        public override bool NextResult()
        {
            _active = false;
            return false;
        }

        public override bool Read()
        {
            if (_active)
            {
                var tmp = _source;
                if (tmp != null && tmp.MoveNext())
                {
                    _current = tmp.Current;
                    return true;
                }
                else
                {
                    _active = false;
                }
            }
            _current = null;
            return false;
        }

        public override int RecordsAffected
        {
            get { return 0; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing) Shutdown();
        }

        private void Shutdown()
        {
            _active = false;
            _current = null;
            var tmp = _source as IDisposable;
            _source = null;
            if (tmp != null) tmp.Dispose();
        }

        public override int FieldCount
        {
            get { return _memberNames.Length; }
        }

        public override bool IsClosed
        {
            get { return _source == null; }
        }

        public override bool GetBoolean(int i)
        {
            return (bool)this[i];
        }

        public override byte GetByte(int i)
        {
            return (byte)this[i];
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var s = (byte[])this[i];
            var available = s.Length - (int)fieldOffset;
            if (available <= 0) return 0;

            var count = Math.Min(length, available);
            Buffer.BlockCopy(s, (int)fieldOffset, buffer, bufferoffset, count);
            return count;
        }

        public override char GetChar(int i)
        {
            return (char)this[i];
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var s = (string)this[i];
            var available = s.Length - (int)fieldoffset;
            if (available <= 0) return 0;

            var count = Math.Min(length, available);
            s.CopyTo((int)fieldoffset, buffer, bufferoffset, count);
            return count;
        }

        protected override DbDataReader GetDbDataReader(int i)
        {
            throw new NotSupportedException();
        }

        public override string GetDataTypeName(int i)
        {
            return (_effectiveTypes == null ? typeof(object) : _effectiveTypes[i]).Name;
        }

        public override DateTime GetDateTime(int i)
        {
            return (DateTime)this[i];
        }

        public override decimal GetDecimal(int i)
        {
            return (decimal)this[i];
        }

        public override double GetDouble(int i)
        {
            return (double)this[i];
        }

        public override Type GetFieldType(int i)
        {
            return _effectiveTypes == null ? typeof(object) : _effectiveTypes[i];
        }

        public override float GetFloat(int i)
        {
            return (float)this[i];
        }

        public override Guid GetGuid(int i)
        {
            return (Guid)this[i];
        }

        public override short GetInt16(int i)
        {
            return (short)this[i];
        }

        public override int GetInt32(int i)
        {
            return (int)this[i];
        }

        public override long GetInt64(int i)
        {
            return (long)this[i];
        }

        public override string GetName(int i)
        {
            return _memberNames[i];
        }

        public override int GetOrdinal(string name)
        {
            return Array.IndexOf(_memberNames, name);
        }

        public override string GetString(int i)
        {
            return (string)this[i];
        }

        public override object GetValue(int i)
        {
            return this[i];
        }

        public override IEnumerator GetEnumerator() => new DbEnumerator(this);

        public override int GetValues(object[] values)
        {
            // duplicate the key fields on the stack
            var members = _memberNames;
            var current = _current;
            var accessor = _accessor;

            var count = Math.Min(values.Length, members.Length);

            for (var i = 0; i < count; i++)
            { values[i] = accessor[current, members[i]] ?? DBNull.Value; }

            return count;
        }

        public override bool IsDBNull(int i)
        {
            return this[i] is DBNull;
        }

        public override object this[string name]
        {
            get { return _accessor[_current, name] ?? DBNull.Value; }
        }

        public override object this[int i]
        {
            get { return _accessor[_current, _memberNames[i]] ?? DBNull.Value; }
        }
    }
}
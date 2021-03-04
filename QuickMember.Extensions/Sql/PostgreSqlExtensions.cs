using SqlKata;
using SqlKata.Compilers;
using System.Collections.Generic;
using System.Linq;

namespace QuickMember.Extensions.Sql
{
    public static class PostgreSqlExtensions
    {
        private readonly static PostgresCompiler _compiler = new PostgresCompiler();
        private readonly static string ID_KEY = "Id";

        private static string SnakeNameConversion(string input)
        {
            return input.ToSnakeCase();
        }

        private readonly static List<string> _globalNameExclusion = new List<string>
        {
            "Id"
        };

        public static string GetSqlCrudSelect<T>(this T input) where T : class, new()
        {
            var properties = input.GetNonEnumerableProperties(_globalNameExclusion, SnakeNameConversion);
            var query = new Query(typeof(T).Name.ToSnakeCase())
                .Select(properties.Keys.ToArray());
            return _compiler.Compile(query).Sql;
        }

        public static string GetSqlCrudSelectInnerJoin<TBase, TJoin>(
            this TBase input,
            TJoin secondInput,
            string fromName,
            string toName,
            string operation) where TBase : class, new() where TJoin : class, new()
        {
            var properties = input.GetNonEnumerableProperties(_globalNameExclusion, SnakeNameConversion);
            var query = new Query(typeof(TBase).Name.ToSnakeCase())
                .Select(properties.Keys.ToArray())
                .Join(
                    typeof(TJoin).Name.ToSnakeCase(),
                    fromName.ToSnakeCase(),
                    toName.ToSnakeCase(),
                    operation);
            return _compiler.Compile(query).Sql;
        }

        public static string GetSqlCrudSelectInnerJoin<TBase, TJoin>(
            this TBase input,
            string joinTo,
            List<string> selects,
            string fromName,
            string toName,
            string operation) where TBase : class, new() where TJoin : class, new()
        {
            var properties = input.GetNonEnumerableProperties(_globalNameExclusion, SnakeNameConversion);
            var query = new Query(typeof(TBase).Name.ToSnakeCase())
                .Select(selects.ToSnakeCases().ToArray())
                .LeftJoin(
                    joinTo.ToSnakeCase(),
                    fromName.ToSnakeCase(),
                    toName.ToSnakeCase(),
                    operation);
            return _compiler.Compile(query).Sql;
        }

        public static string GetSqlCrudSelectLeftJoin<TBase, TJoin>(
            this TBase input,
            string fromName,
            string toName,
            string operation) where TBase : class, new()
        {
            var properties = input.GetNonEnumerableProperties(_globalNameExclusion, SnakeNameConversion);
            var query = new Query(typeof(TBase).Name.ToSnakeCase())
                .Select(properties.Keys.ToArray())
                .LeftJoin(
                    typeof(TJoin).Name.ToSnakeCase(),
                    fromName.ToSnakeCase(),
                    toName.ToSnakeCase(),
                    operation);
            return _compiler.Compile(query).Sql;
        }

        public static string GetSqlCrudSelectLeftJoin<TBase, TJoin>(
            this TBase input,
            string joinTo,
            List<string> selects,
            string fromName,
            string toName,
            string operation) where TBase : class, new()
        {
            var properties = input.GetNonEnumerableProperties(_globalNameExclusion, SnakeNameConversion);
            selects.ToSnakeCases();
            var query = new Query(typeof(TBase).Name.ToSnakeCase())
                .Select(selects.ToArray())
                .Join(
                    joinTo.ToSnakeCase(),
                    fromName.ToSnakeCase(),
                    toName.ToSnakeCase(),
                    operation);
            return _compiler.Compile(query).Sql;
        }

        public static string GetSqlCrudInsert<T>(this T input) where T : class, new()
        {
            var properties = input.GetNonEnumerableProperties(_globalNameExclusion, SnakeNameConversion);
            var query = new Query(typeof(T).Name.ToSnakeCase())
                .AsInsert(properties, true);
            return _compiler.Compile(query).Sql;
        }

        public static string GetSqlCrudInsertWithReturnId<T>(this T input) where T : class, new()
        {
            var properties = input.GetNonEnumerableProperties(_globalNameExclusion, SnakeNameConversion);
            var query = new Query(typeof(T).Name.ToSnakeCase())
                .AsInsert(properties, true);
            return _compiler.Compile(query).Sql;
        }

        public static string GetSqlCrudUpdate<T>(this T input) where T : class, new()
        {
            var properties = input.GetNonEnumerableProperties(_globalNameExclusion, SnakeNameConversion);
            var query = new Query(typeof(T).Name.ToSnakeCase())
                .Where(ID_KEY.ToLower(), input.GetIntId())
                .AsUpdate(properties);
            return _compiler.Compile(query).Sql;
        }

        public static string GetSqlCrudDelete<T>(this T input) where T : class, new()
        {
            var query = new Query(typeof(T).Name.ToSnakeCase())
                .Where(ID_KEY.ToLower(), input.GetIntId())
                .AsDelete();
            return _compiler.Compile(query).Sql;
        }

        public static int GetIntId<T>(this T input) where T : class, new()
        {
            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
            return (int)objectMemberAccessor[input, ID_KEY];
        }

        public static long GetLongId<T>(this T input) where T : class, new()
        {
            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
            return (long)objectMemberAccessor[input, ID_KEY];
        }

        public static string GetStringId<T>(this T input) where T : class, new()
        {
            var objectMemberAccessor = TypeAccessor.Create(typeof(T));
            return (string)objectMemberAccessor[input, ID_KEY];
        }
    }
}

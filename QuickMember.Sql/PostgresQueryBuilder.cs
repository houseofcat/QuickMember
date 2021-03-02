using SqlKata;
using SqlKata.Compilers;

namespace QuickMember.Sql
{
    public static class PostgresQueryBuilder
    {
        private readonly static PostgresCompiler _compiler;

        static PostgresQueryBuilder()
        {
            _compiler = new PostgresCompiler();
        }

        public static string BuildSelect<T>(params object[] args)
        {
            var query = new Query(typeof(T).Name.ToSnakeCase()).Select();

            return _compiler.Compile(query).RawSql;
        }
    }
}

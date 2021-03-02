using SqlKata.Compilers;
using System;

namespace QuickMember.Sql
{
    public static class PostgresQueryBuilder
    {
        private readonly static PostgresCompiler _compiler;

        static PostgresQueryBuilder()
        {
            _compiler = new PostgresCompiler();
        }

        public static string BuildSelect<T>(this T input, params string[] args)
        {
            return string.Empty;
        }
    }
}

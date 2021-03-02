using QuickMember.Sql;
using System;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var result = PostgresQueryBuilder.BuildSelect<TestClass>();

            Console.WriteLine(result);
        }
    }

    public class TestClass
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public decimal Fraction { get; set; }
        public float Float { get; set; }
    }
}

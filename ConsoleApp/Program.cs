using QuickMember.Extensions.Sql;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var testClass = new TestClass();
            var secondTestClass = new SecondTestClass();
            var selectResult = testClass.GetSqlCrudSelect();
            Console.WriteLine("\r\n" + selectResult);

            var select2Result = testClass.GetSqlCrudSelectInnerJoin(
                secondTestClass,
                fromName: nameof(secondTestClass.TestId),
                toName: nameof(testClass.Id),
                "=");

            Console.WriteLine("\r\n" + select2Result);

            var insertResult = testClass.GetSqlCrudInsert();
            Console.WriteLine("\r\n" + insertResult);

            var updateResult = testClass.GetSqlCrudUpdate();
            Console.WriteLine("\r\n" + updateResult);

            var deleteResult = testClass.GetSqlCrudDelete();
            Console.WriteLine("\r\n" + deleteResult);
        }
    }

    public class TestClass
    {
        public int Id { get; set; }
        public string TestDescription { get; set; }
        public string TestData { get; set; }
        public decimal TestFraction { get; set; }
        public float TestFloat { get; set; }
        public List<string> TestStrings { get; set; }
    }

    public class SecondTestClass
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string TestDescription { get; set; }
        public string TestData { get; set; }
        public decimal TestFraction { get; set; }
        public float TestFloat { get; set; }
        public List<string> TestStrings { get; set; }
    }
}

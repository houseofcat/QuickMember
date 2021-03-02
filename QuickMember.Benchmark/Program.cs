using BenchmarkDotNet.Running;
using System;
using System.Linq;

namespace QuickMember.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<QuickMemberPerformance>(new QuickConfig());
            Console.WriteLine();
            // Display a summary to match the output of the original Performance test
            foreach (var report in summary.Reports.OrderBy(r => r.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo))
            {
                Console.WriteLine("{0}: {1:N2} ns", report.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo, report.ResultStatistics.Median);
            }
            Console.WriteLine();
        }
    }
}
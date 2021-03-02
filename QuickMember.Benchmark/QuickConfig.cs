using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;

namespace QuickMember.Benchmark
{
    public class QuickConfig : ManualConfig
    {
        public QuickConfig()
        {
            Add(Job.Default.WithLaunchCount(1));
            Add(StatisticColumn.Median, StatisticColumn.StdDev);
            Add(CsvExporter.Default, MarkdownExporter.Default, MarkdownExporter.GitHub);
            Add(new ConsoleLogger());
        }
    }
}

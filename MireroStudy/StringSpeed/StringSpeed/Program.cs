using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<StringBenchmark>();

[MemoryDiagnoser]
public class StringBenchmark
{
    [Benchmark]
    public void StringBuilder()
    {
        var list = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            var builder = new StringBuilder();
            for (int j = 0; j < 50; j++)
            {
                builder.AppendLine("aa,");
            }

            list.Add(builder.ToString());
        }
    }

    [Benchmark]
    public void StringJoin()
    {
        var list = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            var joins = new List<string>();
            for (int j = 0; j < 50; j++)
            {
                joins.Add("aa");
            }
            list.Add(String.Join(",\n", joins));
        }
    }

    [Benchmark]
    public void ImproveStringBuilder()
    {
        var list = new List<string>();
        var builder = new StringBuilder();
        for (int i = 0; i < 1000; i++)
        {
            builder.Clear();
            for (int j = 0; j < 50; j++)
            {
                builder.AppendLine("aa,");
            }
            builder.AppendLine("aa,");
            list.Add(builder.ToString());
        }
    }

    [Benchmark]
    public void ImproveStringJoin()
    {
        var list = new List<string>();
        var joins = new List<string>();
        for (int i = 0; i < 1000; i++)
        {
            joins.Clear();
            for (int j = 0; j < 50; j++)
            {
                joins.Add("aa");
            }

            list.Add(String.Join(",\n", joins));
        }
    }
}
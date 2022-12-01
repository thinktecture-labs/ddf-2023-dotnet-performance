using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Sample>();

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser(false)]
public class Sample
{
    [Benchmark]
    public string SoDomething()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < 10_000; i++)
        {
            sb.AppendLine($"Adding {i}");
        }

        return sb.ToString();
    }
}

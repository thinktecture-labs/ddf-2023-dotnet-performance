using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<LinqPerformance>();

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser(false)]
public class LinqPerformance
{
    private readonly IEnumerable<int> _itemsArray = Enumerable.Range(1, 100).ToArray();
    private readonly IEnumerable<int> _itemsList = Enumerable.Range(1, 100).ToList();

    [Benchmark]
    public int MaxArray() => _itemsArray.Max();

    [Benchmark]
    public int MinArray() => _itemsArray.Min();

    [Benchmark]
    public int SumArray() => _itemsArray.Sum();

    [Benchmark]
    public double AverageArray() => _itemsArray.Average();

    [Benchmark]
    public int MaxList() => _itemsList.Max();

    [Benchmark]
    public int MinList() => _itemsList.Min();

    [Benchmark]
    public int SumList() => _itemsList.Sum();

    [Benchmark]
    public double AverageList() => _itemsList.Average();
}

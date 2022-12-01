# Benchmarking

```
dotnet new console -n Benchmark
cd Benchmark

; add net6.0 to target frameworks

dotnet add package BenchmarkDotNet
```

```
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Sample>();

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
```

```
dotnet run -f net6.0
dotnet run -f net7.0
```

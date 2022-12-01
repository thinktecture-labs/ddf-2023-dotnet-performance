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
dotnet run -c Release -f net6.0
dotnet run -c Release -f net7.0
```

# Regex

```
dotnet new console -n Regex
cd Regex

; add net6.0 to target frameworks

dotnet add package BenchmarkDotNet
```

```
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<RegexBenchmarks>();

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser(false)]
public partial class RegexBenchmarks
{
    private const string EmailRegex = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";

    private readonly Regex _oldEmailRegex = new(EmailRegex);
    private readonly Regex _oldCompiledEmailRegex = new(EmailRegex, RegexOptions.Compiled);

    [Params("sebastian.gingter@thinktecture.com", "gingter.com")]
    public string PotentialEmail { get; set; } = default!;

    [Benchmark]
    public bool Old_IsMatch_Email()
    {
        return _oldEmailRegex.IsMatch(PotentialEmail);
    }

    [Benchmark]
    public bool Old_Compiled_IsMatch_Email()
    {
        return _oldCompiledEmailRegex.IsMatch(PotentialEmail);
    }

#if NET7_0
    [GeneratedRegex(EmailRegex)]
    private static partial Regex NewGeneratedRegex();

    private readonly Regex _newEmailRegex = NewGeneratedRegex();

    [Benchmark]
    public bool New_Method_IsMatch_Email()
    {
        return NewGeneratedRegex().IsMatch(PotentialEmail);
    }

    [Benchmark]
    public bool New_Field_IsMatch_Email()
    {
        return _newEmailRegex.IsMatch(PotentialEmail);
    }
#else
    [Benchmark]
    public bool New_Method_IsMatch_Email() => false;

    [Benchmark]
    public bool New_Field_IsMatch_Email() => false;
#endif

    [Benchmark]
    public bool Startup_Old_IsMatch_Email()
    {
        return new Regex(EmailRegex).IsMatch(PotentialEmail);
    }

    [Benchmark]
    public bool Startup_Old_Compiled_IsMatch_Email()
    {
        return new Regex(EmailRegex, RegexOptions.Compiled).IsMatch(PotentialEmail);
    }

#if NET7_0
    [Benchmark]
    public bool Startup_New_Method_IsMatch_Email()
    {
        return NewGeneratedRegex().IsMatch(PotentialEmail);
    }
#else
    [Benchmark]
    public bool Startup_New_Method_IsMatch_Email() => false;
#endif
}
```

```
dotnet run -c Release -f net6.0
dotnet run -c Release -f net7.0
```

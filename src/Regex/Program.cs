using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<RegexBenchmarks>();

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser(false)]
public partial class RegexBenchmarks
{
    private const string EmailRegex = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";

    [Params("sebastian.gingter@thinktecture.com", "gingter.com")]
    public string PotentialEmail { get; set; } = default!;

    // Not Cached uncompiled & compiled

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

    // Cached uncompiled & compiled

    private readonly Regex _oldEmailRegex = new(EmailRegex);
    private readonly Regex _oldCompiledEmailRegex = new(EmailRegex, RegexOptions.Compiled);

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

#if NET7_0_OR_GREATER

    [Benchmark]
    public bool Startup_New_Method_IsMatch_Email()
    {
        return NewGeneratedRegex().IsMatch(PotentialEmail);
    }

    [GeneratedRegex(EmailRegex)]
    private static partial Regex NewGeneratedRegex();

    private readonly Regex _newEmailRegex = NewGeneratedRegex();

    [Benchmark]
    public bool New_Cached_IsMatch_Email()
    {
        return _newEmailRegex.IsMatch(PotentialEmail);
    }

#else

    [Benchmark]
    public bool Startup_New_Method_IsMatch_Email() => false;

    [Benchmark]
    public bool New_Cached_IsMatch_Email() => false;

#endif

}

using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ReflectionPerformance>();

public class Person
{
    private int _age;
    internal Person(int age) => _age = age;
    private int GetAge() => _age;
    private void SetAge(int age) => _age = age;
}

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser(false)]
public class ReflectionPerformance
{
    private readonly Person _person = new(10);

    [Benchmark]
    public int GetAge()
    {
        return (int) typeof(Person)
            .GetMethod("GetAge", BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(_person, null)!;
    }

    [Benchmark]
    public void SetAge()
    {
        typeof(Person)
            .GetMethod("SetAge", BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(_person, new object[] { 44, });
    }

    private readonly MethodInfo _getAgeMethod = typeof(Person).GetMethod("GetAge", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private readonly MethodInfo _setAgeMethod = typeof(Person).GetMethod("SetAge", BindingFlags.NonPublic | BindingFlags.Instance)!;

    [Benchmark]
    public int GetAgeCached()
    {
        return (int) _getAgeMethod.Invoke(_person, null)!;
    }

    [Benchmark]
    public void SetAgeCached()
    {
        _setAgeMethod.Invoke(_person, new object[] { 44, });
    }

    private readonly object[] _params = new object[] { 44, };

    [Benchmark]
    public void SetAgeCachedParams()
    {
        _setAgeMethod.Invoke(_person, _params);
    }

    [Benchmark]
    public void Ctor()
    {
        var person = typeof(Person).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, new [] { typeof(int), })!.Invoke(new object[] { 43, });
    }

    private readonly ConstructorInfo _ctor = typeof(Person).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, new [] { typeof(int), })!;

    [Benchmark]
    public void CtorCached()
    {
        var person = _ctor.Invoke(new object[] { 43, });
    }

    [Benchmark]
    public void CtorCachedParams()
    {
        var person = _ctor.Invoke(_params);
    }
}

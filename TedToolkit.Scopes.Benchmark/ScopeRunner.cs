using BenchmarkDotNet.Attributes;

namespace TedToolkit.Scopes.Benchmark;

/// <summary>
/// The scope Runner
/// </summary>
[MemoryDiagnoser]
public class ScopeRunner
{
    /// <summary>
    /// Init
    /// </summary>
    [GlobalSetup]
    public void Init()
    {
        using (new ClassSample(10).Push())
        using (new ClassSample(10).FastPush())
        using (new ValueSample(10).Push())
        using (new ValueSample(10).FastPush())
        using (new TestScope(10))
        {
        }
    }

    /// <summary>
    /// Class Scope
    /// </summary>
    [Benchmark(Baseline = true)]
    public void ScopeBase()
    {
        using (new TestScope(10))
        {
            using (new TestScope(20))
            {
                _ = TestScope.Current?.Value;
            }
        }
    }

    /// <summary>
    /// Class Scope
    /// </summary>
    [Benchmark]
    public void ClassScope()
    {
        using (new ClassSample(10).Push())
        {
            using (new ClassSample(20).Push())
            {
                _ = ScopeValues.Class<ClassSample>.Current?.Value;
            }
        }
    }

    /// <summary>
    /// Value Scope
    /// </summary>
    [Benchmark]
    public void ValueScope()
    {
        using (new ValueSample(10).Push())
        {
            using (new ValueSample(20).Push())
            {
                _ = ScopeValues.Struct<ValueSample>.Current.Value;
            }
        }
    }

    /// <summary>
    /// Fast Class Scope
    /// </summary>
    [Benchmark]
    public void FastClassScope()
    {
        using (new ClassSample(10).FastPush())
        {
            using (new ClassSample(20).FastPush())
            {
                _ = ScopeValues.Class<ClassSample>.Current?.Value;
            }
        }
    }

    /// <summary>
    /// Fast Value Scope
    /// </summary>
    [Benchmark]
    public void FastValueScope()
    {
        using (new ValueSample(10).FastPush())
        {
            using (new ValueSample(20).FastPush())
            {
                _ = ScopeValues.Struct<ValueSample>.Current.Value;
            }
        }
    }
}
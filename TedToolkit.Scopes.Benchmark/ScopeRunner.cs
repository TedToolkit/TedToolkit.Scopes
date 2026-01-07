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
        using (new TestScope(10))
        using (new ValueScope<ValueSample>(new ValueSample(10)))
        using (new FastScope<ValueSample>(new ValueSample(10)))
        {
        }
    }

    /// <summary>
    /// Class Scope
    /// </summary>
    [Benchmark(Baseline = true)]
    public void ClassScope()
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
    /// Value Scope
    /// </summary>
    [Benchmark]
    public void ValueScope()
    {
        using (new ValueScope<ValueSample>(new ValueSample(10)))
        {
            using (new ValueScope<ValueSample>(new ValueSample(20)))
            {
                _ = ValueScope<ValueSample>.Current.Value;
            }
        }
    }

    /// <summary>
    /// Fast Scope
    /// </summary>
    [Benchmark]
    public void FastScope()
    {
        using (new FastScope<ValueSample>(new ValueSample(10)))
        {
            using (new FastScope<ValueSample>(new ValueSample(20)))
            {
                _ = FastScope<ValueSample>.Current.Value;
            }
        }
    }
}
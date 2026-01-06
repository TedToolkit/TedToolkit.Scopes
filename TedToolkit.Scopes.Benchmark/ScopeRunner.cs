using BenchmarkDotNet.Attributes;

namespace TedToolkit.Scopes.Benchmark;

/// <summary>
/// The scope Runner
/// </summary>
[MemoryDiagnoser]
public class ScopeRunner
{
    [Benchmark(Baseline = true)]
    public void ClassScope()
    {
        using (new TestScope(10))
        {
            using (new TestScope(20))
            {
            }
        }
    }

    [Benchmark]
    public void ValueScope()
    {
        using (new Scope<ValueScope>(new ValueScope(10)))
        {
            using (new Scope<ValueScope>(new ValueScope(20)))
            {
            }
        }
    }
}
namespace TedToolkit.Scopes.Benchmark;

internal class TestScope(int value) : ScopeBase<TestScope>
{
    public int Value => value;
}
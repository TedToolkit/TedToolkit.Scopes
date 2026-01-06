namespace TedToolkit.Scopes.Tests;

internal class TestScope(int value) : ScopeBase<TestScope>
{
    public int Value => value;
}
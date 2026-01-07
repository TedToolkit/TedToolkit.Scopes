namespace TedToolkit.Scopes.Tests;

internal class TestValueScope(int value) : ScopeBase<TestValueScope>
{
    public int Value => value;
}
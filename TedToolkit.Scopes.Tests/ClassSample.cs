namespace TedToolkit.Scopes.Tests;

internal class ClassSample(int value) : IScope
{
    public int Value => value;
}
namespace TedToolkit.Scopes.Benchmark;

internal class ClassSample(int value) : IScope
{
    public int Value => value;
}
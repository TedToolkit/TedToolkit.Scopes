namespace TedToolkit.Scopes.Tests;

internal struct ValueSample(int value) : IScope
{
    public int Value => value;
}
using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// Fast Scope Extensions
/// </summary>
public static class FastScopeExtensions
{
    /// <summary>
    /// Push the scope fast.
    /// </summary>
    /// <param name="scope">scope</param>
    /// <typeparam name="TScope">scope type</typeparam>
    /// <returns>scope</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FastScope<TScope> FastPush<TScope>(this TScope scope)
        where TScope : IScope
    {
        return new(scope);
    }
}
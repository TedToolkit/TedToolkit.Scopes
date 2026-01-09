// -----------------------------------------------------------------------
// <copyright file="FastScope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// A fast scope you can't await.
/// </summary>
/// <typeparam name="TScope">scope</typeparam>
public readonly ref struct FastScope<TScope> :
    IDisposable
    where TScope : IScope
{
    [ThreadStatic]
    private static TScope[]? _stack;

    [ThreadStatic]
#pragma warning disable S2743
    private static int _count;
#pragma warning restore S2743

    /// <summary>
    /// Current value
    /// </summary>
    internal static ref readonly TScope RefCurrent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _stack![_count - 1];
    }

    /// <summary>
    /// Current value
    /// </summary>
    public static TScope Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _stack![_count - 1];
    }

    /// <summary>
    /// Has current Value
    /// </summary>
#pragma warning disable RCS1158, S2743
    public static bool HasCurrent
#pragma warning restore RCS1158, S2743
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _count > 0;
    }

    /// <summary>
    /// Create a fast scope
    /// </summary>
    /// <param name="value">value</param>
    public FastScope(scoped in TScope value)
    {
#pragma warning disable S3010
        _stack ??= new TScope[8];
        _count++;
#pragma warning restore S3010

        if (_count >= _stack.Length)
            Array.Resize(ref _stack, _stack.Length + 8);

        _stack[_count - 1] = value;
    }

    /// <inheritdoc />
    public void Dispose()
    {
#pragma warning disable S2696
        _count--;
#pragma warning restore S2696
    }
}
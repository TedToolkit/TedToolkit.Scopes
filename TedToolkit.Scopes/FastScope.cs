// -----------------------------------------------------------------------
// <copyright file="FastScope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// A high-performance thread-local scope backed by a thread-static stack.
/// This is a <see langword="ref struct"/> and cannot be used across <see langword="await"/> boundaries.
/// </summary>
/// <typeparam name="TScope">The type implementing <see cref="IScope"/>.</typeparam>
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
    /// Gets a reference to the current scope value.
    /// </summary>
    internal static ref readonly TScope RefCurrent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return ref _stack![_count - 1];
        }
    }

    /// <summary>
    /// Gets the current scope value.
    /// </summary>
    public static TScope Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _stack![_count - 1];
        }
    }

    /// <summary>
    /// Gets a value indicating whether a current scope exists.
    /// </summary>
#pragma warning disable RCS1158, S2743
    public static bool HasCurrent
#pragma warning restore RCS1158, S2743
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _count > 0;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FastScope{TScope}"/> struct.
    /// </summary>
    /// <param name="value">The scope value to push onto the thread-local stack.</param>
    public FastScope(scoped in TScope value)
    {
#pragma warning disable S3010
        _stack ??= new TScope[8];
        _count++;
#pragma warning restore S3010

        if (_count >= _stack.Length)
        {
            Array.Resize(ref _stack, _stack.Length + 8);
        }

        value.OnEntry();
        _stack[_count - 1] = value;
    }

    /// <inheritdoc />
    public void Dispose()
    {
#pragma warning disable S2696
        _count--;
#pragma warning restore S2696

        _stack?[_count].OnExit();
    }
}
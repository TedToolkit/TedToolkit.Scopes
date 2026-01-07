// -----------------------------------------------------------------------
// <copyright file="ValueScope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// A Scope but not fast
/// </summary>
/// <typeparam name="TScope">Scope type</typeparam>
#pragma warning disable CA1815
public readonly struct ValueScope<TScope> : IDisposable
#pragma warning restore CA1815
    where TScope : struct
{
    private sealed class ScopeNode(scoped in TScope value)
    {
#pragma warning disable SA1401
        public readonly TScope Value = value;
#pragma warning restore SA1401
    }

#pragma warning disable S2743
    private static readonly AsyncLocal<ScopeNode?> _current = new();
#pragma warning restore S2743

    /// <summary>
    /// Current
    /// </summary>
    public static ref readonly TScope Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _current.Value!.Value;
    }

    /// <summary>
    /// Has Value
    /// </summary>
#pragma warning disable RCS1158, S2743
    public static bool HasCurrent
#pragma warning restore RCS1158, S2743
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _current.Value is not null;
    }

    private readonly ScopeNode? _previousNode;

    /// <summary>
    /// Create a scope.
    /// </summary>
    /// <param name="value">value</param>
    public ValueScope(scoped in TScope value)
    {
        _previousNode = _current.Value;
        _current.Value = new(value);
    }

    /// <inheritdoc />
    public void Dispose()
        => _current.Value = _previousNode;
}
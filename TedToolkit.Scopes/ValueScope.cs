// -----------------------------------------------------------------------
// <copyright file="ValueScope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.Refly;

namespace TedToolkit.Scopes;

/// <summary>
/// A Scope but not fast.
/// </summary>
/// <typeparam name="TScope">Scope type.</typeparam>
public readonly record struct ValueScope<TScope> : IDisposable
    where TScope : struct, IScope
{
#pragma warning disable S2743
    private static readonly AsyncLocal<Ref<TScope>?> _current = new();
#pragma warning restore S2743

    /// <summary>
    /// Gets current.
    /// </summary>
    public static ref readonly TScope Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _current.Value!.Value;
    }

    /// <summary>
    /// Gets a value indicating whether has Value.
    /// </summary>
#pragma warning disable RCS1158, S2743
    public static bool HasCurrent
#pragma warning restore RCS1158, S2743
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _current.Value is not null;
    }

    private readonly Ref<TScope>? _previousNode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueScope{TScope}"/> struct.
    /// Create a scope.
    /// </summary>
    /// <param name="value">value.</param>
    public ValueScope(scoped in TScope value)
    {
        _previousNode = _current.Value;
        _current.Value = new(in value);
    }

    /// <inheritdoc />
    public void Dispose()
        => _current.Value = _previousNode;
}
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
/// An async-safe scope wrapper for struct types implementing <see cref="IScope"/>.
/// Uses <see cref="AsyncLocal{T}"/> to flow through <see langword="async"/>/<see langword="await"/> boundaries.
/// </summary>
/// <typeparam name="TScope">The struct type implementing <see cref="IScope"/>.</typeparam>
public readonly record struct ValueScope<TScope> : IDisposable
    where TScope : struct, IScope
{
#pragma warning disable S2743
    private static readonly AsyncLocal<Ref<TScope>?> _current = new();
#pragma warning restore S2743

    /// <summary>
    /// Gets a reference to the current scope value.
    /// </summary>
    public static ref readonly TScope Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return ref _current.Value!.Value;
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
            return _current.Value is not null;
        }
    }

    private readonly Ref<TScope>? _previousNode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueScope{TScope}"/> struct.
    /// </summary>
    /// <param name="value">The scope value to push as current.</param>
    public ValueScope(scoped in TScope value)
    {
        _previousNode = _current.Value;
        var scope = new Ref<TScope>(in value);
        scope.Value.OnEntry();
        _current.Value = scope;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _current.Value?.Value.OnExit();
        _current.Value = _previousNode;
    }
}
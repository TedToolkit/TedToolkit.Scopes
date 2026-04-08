// -----------------------------------------------------------------------
// <copyright file="Scope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// An async-safe scope wrapper for class types implementing <see cref="IScope"/>.
/// Uses <see cref="AsyncLocal{T}"/> to flow through <see langword="async"/>/<see langword="await"/> boundaries.
/// </summary>
/// <typeparam name="TScope">The class type implementing <see cref="IScope"/>.</typeparam>
public readonly record struct Scope<TScope> :
    IDisposable
    where TScope : class, IScope
{
    private static readonly AsyncLocal<TScope?> _current = new();

    private readonly TScope? _parent;

    /// <summary>
    /// Gets the current scope instance, or <see langword="null"/> if no scope is active.
    /// </summary>
    public static TScope? Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _current.Value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Scope{TScope}"/> struct.
    /// </summary>
    /// <param name="scope">The scope instance to push as current.</param>
    public Scope(TScope scope)
    {
        _parent = _current.Value;
        scope?.OnEntry();
        _current.Value = scope;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _current.Value?.OnExit();
        _current.Value = _parent;
    }
}
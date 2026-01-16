// -----------------------------------------------------------------------
// <copyright file="Scope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// Scope base.
/// </summary>
/// <typeparam name="TScope">scope.</typeparam>
public readonly record struct Scope<TScope> :
    IDisposable
    where TScope : class, IScope
{
    private static readonly AsyncLocal<TScope?> _current = new();

    private readonly TScope? _parent;

    /// <summary>
    ///  Gets current Value.
    /// </summary>
    public static TScope? Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _current.Value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Scope{TScope}"/> struct.
    /// Create a base scope.
    /// </summary>
    /// <param name="scope">scope.</param>
    public Scope(TScope scope)
    {
        _parent = _current.Value;
        _current.Value = scope;
    }

    /// <inheritdoc />
    public void Dispose()
        => _current.Value = _parent;
}
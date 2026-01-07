// -----------------------------------------------------------------------
// <copyright file="ScopeRecord.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// Scope base
/// </summary>
/// <typeparam name="TScope">scope</typeparam>
public abstract record ScopeRecord<TScope> :
    IDisposable
    where TScope : ScopeRecord<TScope>
{
    private static readonly AsyncLocal<TScope?> _currentScope = new();

    private readonly TScope? _parent;

    /// <summary>
    ///  Current Value
    /// </summary>
    public static TScope? Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _currentScope.Value;
    }

    /// <summary>
    /// Create a base scope
    /// </summary>
    protected ScopeRecord()
    {
        _parent = _currentScope.Value;
        _currentScope.Value = (TScope)this;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// On Dispose
    /// </summary>
    /// <param name="disposing">disposing</param>
    protected virtual void Dispose(bool disposing)
        => _currentScope.Value = _parent;
}
// -----------------------------------------------------------------------
// <copyright file="ScopeRecord.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// An abstract record base class for scopes, providing async-safe ambient context tracking via <see cref="AsyncLocal{T}"/>.
/// The scope is entered automatically on construction and exited on disposal.
/// </summary>
/// <typeparam name="TScope">The concrete scope type that derives from this record.</typeparam>
public abstract record ScopeRecord<TScope> :
    IDisposable
    where TScope : ScopeRecord<TScope>
{
    private static readonly AsyncLocal<TScope?> _currentScope = new();

    private readonly TScope? _parent;

    /// <summary>
    /// Gets the current scope instance, or <see langword="null"/> if no scope is active.
    /// </summary>
    public static TScope? Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _currentScope.Value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScopeRecord{TScope}"/> class.
    /// </summary>
    protected ScopeRecord()
    {
        _parent = _currentScope.Value;
        _currentScope.Value = (TScope)this;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="ScopeRecord{TScope}"/> class.
    /// </summary>
    ~ScopeRecord()
    {
        Dispose(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases resources and restores the parent scope.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release managed resources; <see langword="false"/> when called from the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _currentScope.Value = _parent;
    }
}
// -----------------------------------------------------------------------
// <copyright file="ValueScopeExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// Extension methods for pushing struct-based <see cref="IScope"/> implementations.
/// </summary>
public static class ValueScopeExtensions
{
    /// <summary>
    /// Pushes the scope onto the thread-local stack for fast, synchronous-only access.
    /// </summary>
    /// <param name="scope">The scope instance to push.</param>
    /// <typeparam name="TScope">The struct type implementing <see cref="IScope"/>.</typeparam>
    /// <returns>A <see cref="FastScope{TScope}"/> that restores the previous scope on disposal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FastScope<TScope> FastPush<TScope>(this TScope scope)
        where TScope : struct, IScope
    {
        return new(scope);
    }

    /// <summary>
    /// Pushes the scope as the current ambient context. Safe to use across <see langword="await"/> boundaries.
    /// Prefer <see cref="FastPush{TScope}"/> in synchronous methods for better performance.
    /// </summary>
    /// <param name="scope">The scope instance to push.</param>
    /// <typeparam name="TScope">The struct type implementing <see cref="IScope"/>.</typeparam>
    /// <returns>A <see cref="ValueScope{TScope}"/> that restores the previous scope on disposal.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueScope<TScope> Push<TScope>(this TScope scope)
        where TScope : struct, IScope
    {
        return new(scope);
    }
}
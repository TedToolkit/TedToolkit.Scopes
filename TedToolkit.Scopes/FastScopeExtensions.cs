// -----------------------------------------------------------------------
// <copyright file="FastScopeExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// Fast Scope Extensions
/// </summary>
public static class FastScopeExtensions
{
    /// <summary>
    /// Push the scope fast.
    /// </summary>
    /// <param name="scope">scope</param>
    /// <typeparam name="TScope">scope type</typeparam>
    /// <returns>scope</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FastScope<TScope> FastPush<TScope>(this TScope scope)
        where TScope : IScope
    {
        return new(scope);
    }
}
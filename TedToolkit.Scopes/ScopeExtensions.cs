// -----------------------------------------------------------------------
// <copyright file="ScopeExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// The extensions for the scope
/// </summary>
public static class ScopeExtensions
{
    /// <summary>
    /// Push the scope
    /// </summary>
    /// <param name="scope">scope</param>
    /// <typeparam name="TScope">scope type</typeparam>
    /// <returns>scope</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Scope<TScope> Push<TScope>(this TScope scope)
        where TScope : class, IScope
    {
        return new(scope);
    }
}
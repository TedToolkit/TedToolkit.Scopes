// -----------------------------------------------------------------------
// <copyright file="ValueScopeExtension.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

/// <summary>
/// The Scope extension.
/// </summary>
public static class ValueScopeExtension
{
    /// <summary>
    /// Push the scope. Use the fast one as possible.
    /// </summary>
    /// <param name="scope">scope</param>
    /// <typeparam name="TScope">scope type</typeparam>
    /// <returns>scope</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueScope<TScope> Push<TScope>(this TScope scope)
        where TScope : struct, IScope
    {
        return new(scope);
    }
}
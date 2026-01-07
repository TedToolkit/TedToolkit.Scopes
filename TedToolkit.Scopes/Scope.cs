// -----------------------------------------------------------------------
// <copyright file="Scope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

#pragma warning disable CA1034

/// <summary>
/// Get the scope value
/// </summary>
public static class Scope
{
    /// <summary>
    /// Get the value scope.
    /// </summary>
    /// <typeparam name="TScope">scope type</typeparam>
    public static class Value<TScope>
        where TScope : struct
    {
        /// <summary>
        /// Current value
        /// </summary>
        public static ref readonly TScope Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref FastScope<TScope>.HasCurrent ? ref FastScope<TScope>.Current : ref ValueScope<TScope>.Current;
        }

        /// <summary>
        /// Has Value
        /// </summary>
        public static bool HasValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => FastScope<TScope>.HasCurrent || ValueScope<TScope>.HasCurrent;
        }
    }

    /// <summary>
    /// Get the class scope
    /// </summary>
    /// <typeparam name="TScope">scope type</typeparam>
    public static class Class<TScope>
        where TScope : ScopeBase<TScope>
    {
        /// <summary>
        /// Current Value
        /// </summary>
        public static TScope? Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => FastScope<TScope>.HasCurrent ? FastScope<TScope>.Current : ScopeBase<TScope>.Current;
        }
    }

    /// <summary>
    /// Get the record type
    /// </summary>
    /// <typeparam name="TScope">scope type</typeparam>
    public static class Record<TScope>
        where TScope : ScopeRecord<TScope>
    {
        /// <summary>
        /// Current Value
        /// </summary>
        public static TScope? Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => FastScope<TScope>.HasCurrent ? FastScope<TScope>.Current : ScopeRecord<TScope>.Current;
        }
    }
}
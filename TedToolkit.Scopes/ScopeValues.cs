// -----------------------------------------------------------------------
// <copyright file="ScopeValues.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Scopes;

#pragma warning disable CA1034

/// <summary>
/// Get the value scope.
/// </summary>
public static class ScopeValues
{
    /// <summary>
    /// Struct scope.
    /// </summary>
    /// <typeparam name="TScope">scope type.</typeparam>
    public static class Struct<TScope>
        where TScope : struct, IScope
    {
        /// <summary>
        /// Gets current value.
        /// </summary>
        public static ref readonly TScope Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref FastScope<TScope>.HasCurrent ? ref FastScope<TScope>.RefCurrent : ref ValueScope<TScope>.Current;
        }

        /// <summary>
        /// Gets a value indicating whether has Current.
        /// </summary>
        public static bool HasCurrent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => FastScope<TScope>.HasCurrent || ValueScope<TScope>.HasCurrent;
        }
    }

    /// <summary>
    /// Class scope.
    /// </summary>
    /// <typeparam name="TScope">scope type.</typeparam>
    public static class Class<TScope>
        where TScope : class, IScope
    {
        /// <summary>
        /// Gets current value.
        /// </summary>
        public static TScope? Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => FastScope<TScope>.HasCurrent ? FastScope<TScope>.Current : Scope<TScope>.Current;
        }

        /// <summary>
        /// Gets a value indicating whether has Current.
        /// </summary>
        public static bool HasCurrent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => FastScope<TScope>.HasCurrent || Scope<TScope>.Current is not null;
        }
    }
}
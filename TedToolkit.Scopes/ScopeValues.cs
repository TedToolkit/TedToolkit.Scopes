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
/// Provides unified access to the current scope, checking both <see cref="FastScope{TScope}"/> and async-safe scopes.
/// </summary>
public static class ScopeValues
{
    /// <summary>
    /// Provides access to the current scope for struct types.
    /// </summary>
    /// <typeparam name="TScope">The struct type implementing <see cref="IScope"/>.</typeparam>
    public static class Struct<TScope>
        where TScope : struct, IScope
    {
        /// <summary>
        /// Gets a reference to the current scope value. Checks <see cref="FastScope{TScope}"/> first, then <see cref="ValueScope{TScope}"/>.
        /// </summary>
        public static ref readonly TScope Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return ref FastScope<TScope>.HasCurrent ? ref FastScope<TScope>.RefCurrent : ref ValueScope<TScope>.Current;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a current scope exists.
        /// </summary>
        public static bool HasCurrent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return FastScope<TScope>.HasCurrent || ValueScope<TScope>.HasCurrent;
            }
        }
    }

    /// <summary>
    /// Provides access to the current scope for class types.
    /// </summary>
    /// <typeparam name="TScope">The class type implementing <see cref="IScope"/>.</typeparam>
    public static class Class<TScope>
        where TScope : class, IScope
    {
        /// <summary>
        /// Gets the current scope value. Checks <see cref="FastScope{TScope}"/> first, then <see cref="Scope{TScope}"/>.
        /// </summary>
        public static TScope? Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return FastScope<TScope>.HasCurrent ? FastScope<TScope>.Current : Scope<TScope>.Current;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a current scope exists.
        /// </summary>
        public static bool HasCurrent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return FastScope<TScope>.HasCurrent || Scope<TScope>.Current is not null;
            }
        }
    }
}
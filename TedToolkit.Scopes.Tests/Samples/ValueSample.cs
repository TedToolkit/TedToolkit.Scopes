// -----------------------------------------------------------------------
// <copyright file="ValueSample.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Tests.Samples;

/// <summary>
/// A struct-based scope sample for testing.
/// </summary>
/// <param name="value">The scope value.</param>
/// <param name="action">An optional action to invoke on exit.</param>
internal struct ValueSample(int value, Action? action = null) : IScope
{
    /// <summary>
    /// Gets the scope value.
    /// </summary>
    public int Value
    {
        get
        {
            return value;
        }
    }

    /// <inheritdoc/>
    public void OnEntry()
    {
    }

    /// <inheritdoc/>
    public void OnExit()
    {
        action?.Invoke();
    }
}
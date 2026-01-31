// -----------------------------------------------------------------------
// <copyright file="ValueSample.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Tests.Samples;

/// <summary>
/// Struct.
/// </summary>
/// <param name="value">value.</param>
/// <param name="action">action.</param>
internal struct ValueSample(int value, Action? action = null) : IScope
{
    /// <summary>
    /// Gets value.
    /// </summary>
    public int Value
        => value;

    /// <inheritdoc/>
    public void OnEntry()
    {
    }

    /// <inheritdoc/>
    public void OnExit()
        => action?.Invoke();
}
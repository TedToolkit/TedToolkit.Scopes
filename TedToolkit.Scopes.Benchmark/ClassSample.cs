// -----------------------------------------------------------------------
// <copyright file="ClassSample.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Benchmark;

/// <summary>
/// A class-based scope sample for benchmarking.
/// </summary>
/// <param name="value">The scope value.</param>
internal sealed class ClassSample(int value) : IScope
{
    /// <summary>
    /// Gets the scope value.
    /// </summary>
    public int Value
        => value;

    /// <inheritdoc/>
    public void OnEntry()
    {
    }

    /// <inheritdoc/>
    public void OnExit()
    {
    }
}
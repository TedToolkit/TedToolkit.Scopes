// -----------------------------------------------------------------------
// <copyright file="ClassSample.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Tests.Samples;

/// <summary>
/// A class-based scope sample for testing.
/// </summary>
/// <param name="value">The scope value.</param>
internal sealed class ClassSample(int value) : IScope
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

    /// <summary>
    /// Gets a value indicating whether the scope has been exited.
    /// </summary>
    public bool Exited { get; private set; }

    /// <inheritdoc/>
    public void OnEntry()
    {
    }

    /// <inheritdoc/>
    public void OnExit()
    {
        Exited = true;
    }
}
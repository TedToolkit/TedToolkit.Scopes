// -----------------------------------------------------------------------
// <copyright file="ClassSample.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Benchmark;

/// <summary>
/// Class sample
/// </summary>
/// <param name="value">value</param>
internal sealed class ClassSample(int value) : IScope
{
    /// <summary>
    /// Value
    /// </summary>
    public int Value
        => value;
}
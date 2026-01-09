// -----------------------------------------------------------------------
// <copyright file="ValueSample.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Tests;

/// <summary>
/// Struct
/// </summary>
/// <param name="value">value</param>
internal struct ValueSample(int value) : IScope
{
    /// <summary>
    /// Value
    /// </summary>
    public int Value
        => value;
}
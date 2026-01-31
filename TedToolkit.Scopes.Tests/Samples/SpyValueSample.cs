// -----------------------------------------------------------------------
// <copyright file="SpyValueSample.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Tests.Samples;

/// <summary>
/// Struct.
/// </summary>
/// <param name="action">action.</param>
internal struct SpyValueSample(Action action) : IExitActionScope
{
    /// <inheritdoc/>
    public void OnExit()
        => action();
}
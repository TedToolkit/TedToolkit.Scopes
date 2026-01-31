// -----------------------------------------------------------------------
// <copyright file="SpyClassSample.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Tests.Samples;

/// <summary>
/// Class.
/// </summary>
internal sealed class SpyClassSample : IExitActionScope
{
    /// <summary>
    /// Excited
    /// </summary>
    public bool Exited { get; private set; }

    /// <inheritdoc/>
    public void OnExit()
        => Exited = true;
}
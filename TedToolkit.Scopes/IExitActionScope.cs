// -----------------------------------------------------------------------
// <copyright file="IExitActionScope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes;

/// <summary>
/// The scope that can exit.
/// </summary>
public interface IExitActionScope : IScope
{
    /// <summary>
    /// The exit action.
    /// </summary>
    void OnExit();
}
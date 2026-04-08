// -----------------------------------------------------------------------
// <copyright file="IScope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes;

/// <summary>
/// Defines the lifecycle methods for a scope.
/// </summary>
public interface IScope
{
    /// <summary>
    /// Called when the scope is entered.
    /// </summary>
    void OnEntry();

    /// <summary>
    /// Called when the scope is exited.
    /// </summary>
    void OnExit();
}
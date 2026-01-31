// -----------------------------------------------------------------------
// <copyright file="IScope.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes;

/// <summary>
/// The scope.
/// </summary>
public interface IScope
{
    /// <summary>
    /// The entry action.
    /// </summary>
    void OnEntry();

    /// <summary>
    /// The exit action.
    /// </summary>
    void OnExit();
}
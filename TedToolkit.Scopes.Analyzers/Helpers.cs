// -----------------------------------------------------------------------
// <copyright file="Helpers.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TedToolkit.Scopes.Analyzers;

/// <summary>
/// Some helpers for the analyzer.
/// </summary>
internal static class Helpers
{
    /// <summary>
    /// Is in the async context
    /// </summary>
    /// <param name="node">node</param>
    /// <returns>is in async</returns>
    public static bool IsInAsyncContext(this SyntaxNode node)
    {
        var parent = node.Ancestors().FirstOrDefault(n =>
            n is MethodDeclarationSyntax or LocalFunctionStatementSyntax or AnonymousFunctionExpressionSyntax);

        if (parent is null)
            return false;

        return parent switch
        {
            MethodDeclarationSyntax m => m.Modifiers.Any(SyntaxKind.AsyncKeyword),
            LocalFunctionStatementSyntax l => l.Modifiers.Any(SyntaxKind.AsyncKeyword),
            AnonymousFunctionExpressionSyntax a => a.AsyncKeyword.IsKind(SyntaxKind.AsyncKeyword),
            _ => false,
        };
    }
}
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
/// Helper methods for the scope analyzers.
/// </summary>
internal static class Helpers
{
    /// <summary>
    /// Determines whether the given syntax node is inside an async context.
    /// </summary>
    /// <param name="node">The syntax node to check.</param>
    /// <returns><see langword="true"/> if the node is inside an async method, local function, or lambda; otherwise, <see langword="false"/>.</returns>
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
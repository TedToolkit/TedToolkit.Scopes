// -----------------------------------------------------------------------
// <copyright file="ScopeCodeFixProvider.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Immutable;
using System.Composition;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TedToolkit.Scopes.Analyzers;

/// <summary>
/// The fixer for the scope code
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ScopeCodeFixProvider))]
[Shared]
public class ScopeCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc />
    public sealed override ImmutableArray<string> FixableDiagnosticIds
        => [ScopeAnalyzer.DIAGNOSTIC_ID,];

    /// <inheritdoc />
    public sealed override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var declaration = root.FindToken(diagnosticSpan.Start).Parent
            ?.AncestorsAndSelf()
            .OfType<ObjectCreationExpressionSyntax>().First();

        if (declaration is null)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Convert to FastScope<T>",
                createChangedDocument: c => ReplaceWithFastScopeAsync(context.Document, declaration, c),
                equivalenceKey: nameof(ScopeCodeFixProvider)),
            diagnostic);
    }

    private static async Task<Document> ReplaceWithFastScopeAsync(Document document, ObjectCreationExpressionSyntax creation,
        CancellationToken cancellationToken)
    {
        if (creation.Type is not GenericNameSyntax genericName)
            return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        var newIdentifier = SyntaxFactory.Identifier("FastScope");
        var newGenericName = genericName.WithIdentifier(newIdentifier);
        var newCreation = creation.WithType(newGenericName);

        var newRoot = root.ReplaceNode(creation, newCreation);
        return document.WithSyntaxRoot(newRoot);
    }
}
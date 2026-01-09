// -----------------------------------------------------------------------
// <copyright file="ScopePushCodeFixProvider.cs" company="TedToolkit">
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
/// The push method fixer
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ScopePushCodeFixProvider))]
[Shared]
public class ScopePushCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds
        => [ScopePushAnalyzer.DIAGNOSTIC_ID,];

    /// <inheritdoc />
    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        if (root.FindToken(diagnosticSpan.Start).Parent is not SimpleNameSyntax identifier)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Switch to 'FastPush'",
                createChangedDocument: c => ReplaceWithFastPushAsync(context.Document, identifier, c),
                equivalenceKey: "SwitchToFastPush"),
            diagnostic);
    }

    private static async Task<Document> ReplaceWithFastPushAsync(Document document, SimpleNameSyntax oldIdentifier,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
            return document;

        TypeSyntax newIdentifier = oldIdentifier is GenericNameSyntax genericName
            ? SyntaxFactory.GenericName(
                SyntaxFactory.Identifier("FastPush"),
                genericName.TypeArgumentList)
            : SyntaxFactory.IdentifierName("FastPush");

        var newRoot = root.ReplaceNode(oldIdentifier, newIdentifier);
        return document.WithSyntaxRoot(newRoot);
    }
}
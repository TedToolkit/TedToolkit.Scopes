// -----------------------------------------------------------------------
// <copyright file="ScopeAnalyzer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace TedToolkit.Scopes.Analyzers;

/// <summary>
/// For the scope creation check.
/// </summary>
#pragma warning disable RS1038
[DiagnosticAnalyzer(LanguageNames.CSharp)]
#pragma warning restore RS1038
public class ScopeAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Diagnostic id
    /// </summary>
    public const string DIAGNOSTIC_ID = "SCOPE001";

    private const string CATEGORY = "Performance";

    private static readonly LocalizableString _title = "Use FastScope in non-async contexts";

    private static readonly LocalizableString _messageFormat =
        "Type '{0}' should be replaced with 'FastScope<{1}>' when used outside of an async context";

    private static readonly LocalizableString _description =
        "FastScope provides better performance for synchronous execution environments.";

    private static readonly DiagnosticDescriptor _rule = new(
        DIAGNOSTIC_ID, _title, _messageFormat, CATEGORY,
        DiagnosticSeverity.Info, isEnabledByDefault: true,
        description: _description);

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [_rule,];

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(compilationContext =>
        {
            var scopeTypeSymbol = compilationContext.Compilation.GetTypeByMetadataName("TedToolkit.Scopes.Scope`1");
            var valueScopeTypeSymbol =
                compilationContext.Compilation.GetTypeByMetadataName("TedToolkit.Scopes.ValueScope`1");

            if (scopeTypeSymbol is null || valueScopeTypeSymbol is null)
                return;

            compilationContext.RegisterSyntaxNodeAction(
                nodeContext => AnalyzeObjectCreation(nodeContext, scopeTypeSymbol, valueScopeTypeSymbol),
                SyntaxKind.ObjectCreationExpression);
        });
    }

    private static void AnalyzeObjectCreation(scoped in SyntaxNodeAnalysisContext context,
        params IReadOnlyList<INamedTypeSymbol> scopeTypeSymbols)
    {
        var objectCreation = (ObjectCreationExpressionSyntax)context.Node;
        var symbolInfo = context.SemanticModel.GetSymbolInfo(objectCreation, context.CancellationToken);

        if (symbolInfo.Symbol is not IMethodSymbol constructorSymbol)
            return;

        var type = constructorSymbol.ContainingType;
        if (!scopeTypeSymbols.Any(scopeTypeSymbol =>
                SymbolEqualityComparer.Default.Equals(type.OriginalDefinition, scopeTypeSymbol)))
        {
            return;
        }

        if (objectCreation.IsInAsyncContext())
            return;

        var typeArgument = type.TypeArguments[0].Name;
        var diagnostic = Diagnostic.Create(_rule, objectCreation.GetLocation(), type.Name, typeArgument);
        context.ReportDiagnostic(diagnostic);
    }
}
// -----------------------------------------------------------------------
// <copyright file="ScopePushAnalyzer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Analyzers;

using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// The scope push method analyzer
/// </summary>
#pragma warning disable RS1038
[DiagnosticAnalyzer(LanguageNames.CSharp)]
#pragma warning restore RS1038
public class ScopePushAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Diagnostic id
    /// </summary>
    public const string DIAGNOSTIC_ID = "SCOPE002";

    private const string CATEGORY = "Performance";

    private static readonly LocalizableString _title = "Use FastPush in non-async contexts";

    private static readonly LocalizableString _messageFormat =
        "Method 'Push' is called in a non-async context. Use 'FastPush' instead.";

    private static readonly LocalizableString _description =
        "Push<TScope> is optimized for async tracking. In synchronous methods, FastPush<TScope> is preferred.";

    private static readonly DiagnosticDescriptor _rule = new(
        DIAGNOSTIC_ID, _title, _messageFormat, CATEGORY,
        DiagnosticSeverity.Info, isEnabledByDefault: true, description: _description);

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
            var pushMethodSymbol = GetPushMethod("TedToolkit.Scopes.ScopeExtensions");
            var valuePushMethodSymbol = GetPushMethod("TedToolkit.Scopes.ValueScopeExtensions");

            if (pushMethodSymbol is null || valuePushMethodSymbol is null)
                return;

            compilationContext.RegisterSyntaxNodeAction(nodeContext =>
                    AnalyzeNode(nodeContext, pushMethodSymbol, valuePushMethodSymbol),
                SyntaxKind.InvocationExpression);

            IMethodSymbol? GetPushMethod(string metaName)
            {
                return compilationContext.Compilation.GetTypeByMetadataName(metaName)
                    ?.GetMembers("Push")
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => m.IsGenericMethod)?.OriginalDefinition;
            }
        });
    }

    private static void AnalyzeNode(scoped in SyntaxNodeAnalysisContext context,
        params IReadOnlyList<IMethodSymbol> targetPushSymbols)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken);

        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
            return;

        methodSymbol = methodSymbol.ReducedFrom ?? methodSymbol;
        if (!targetPushSymbols.Any(targetPushSymbol =>
                SymbolEqualityComparer.Default.Equals(methodSymbol.OriginalDefinition, targetPushSymbol)))
        {
            return;
        }

        if (invocation.IsInAsyncContext())
            return;

        var location = invocation.GetLocation();

        if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            location = memberAccess.Name.GetLocation();

        context.ReportDiagnostic(Diagnostic.Create(_rule, location));
    }
}
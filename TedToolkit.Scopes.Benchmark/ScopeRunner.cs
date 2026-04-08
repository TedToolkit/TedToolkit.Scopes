// -----------------------------------------------------------------------
// <copyright file="ScopeRunner.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using BenchmarkDotNet.Attributes;

namespace TedToolkit.Scopes.Benchmark;

#pragma warning disable S108, CA1822, CA1515

/// <summary>
/// Benchmark runner for comparing scope implementations.
/// </summary>
[MemoryDiagnoser]
public class ScopeRunner
{
    /// <summary>
    /// Warms up all scope implementations before benchmarking.
    /// </summary>
    [GlobalSetup]
    public void Init()
    {
        using (new ClassSample(10).Push())
        using (new ClassSample(10).FastPush())
        using (new ValueSample(10).Push())
        using (new ValueSample(10).FastPush())
        using (new TestScope(10))
        {
        }
    }

    /// <summary>
    /// Benchmarks <see cref="ScopeBase{TScope}"/> (baseline).
    /// </summary>
    [Benchmark(Baseline = true)]
    public void ScopeBase()
    {
        using (new TestScope(10))
        using (new TestScope(20))
            _ = TestScope.Current?.Value;
    }

    /// <summary>
    /// Benchmarks <see cref="Scope{TScope}"/> via <see cref="ScopeExtensions.Push{TScope}"/>.
    /// </summary>
    [Benchmark]
    public void ClassScope()
    {
        using (new ClassSample(10).Push())
        using (new ClassSample(20).Push())
            _ = ScopeValues.Class<ClassSample>.Current?.Value;
    }

    /// <summary>
    /// Benchmarks <see cref="ValueScope{TScope}"/> via <see cref="ValueScopeExtensions.Push{TScope}"/>.
    /// </summary>
    [Benchmark]
    public void ValueScope()
    {
        using (new ValueSample(10).Push())
        using (new ValueSample(20).Push())
            _ = ScopeValues.Struct<ValueSample>.Current.Value;
    }

    /// <summary>
    /// Benchmarks <see cref="FastScope{TScope}"/> with a class type via <see cref="ScopeExtensions.FastPush{TScope}"/>.
    /// </summary>
    [Benchmark]
    public void FastClassScope()
    {
        using (new ClassSample(10).FastPush())
        using (new ClassSample(20).FastPush())
            _ = ScopeValues.Class<ClassSample>.Current?.Value;
    }

    /// <summary>
    /// Benchmarks <see cref="FastScope{TScope}"/> with a struct type via <see cref="ValueScopeExtensions.FastPush{TScope}"/>.
    /// </summary>
    [Benchmark]
    public void FastValueScope()
    {
        using (new ValueSample(10).FastPush())
        using (new ValueSample(20).FastPush())
            _ = ScopeValues.Struct<ValueSample>.Current.Value;
    }
}
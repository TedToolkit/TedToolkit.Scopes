// -----------------------------------------------------------------------
// <copyright file="ScopeTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Scopes.Tests;

/// <summary>
/// Scope tests.
/// </summary>
internal sealed class ScopeTests
{
    /// <summary>
    /// Scope base test.
    /// </summary>
    /// <param name="count">count.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    [MatrixDataSource]
    public async Task ScopeBaseTasksTestAsync(
        [Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        int count)
    {
        var baseCount = count * 10;
        using (new ClassSample(baseCount + 1).Push())
        {
            await Assert.That(ScopeValues.Class<ClassSample>.Current?.Value).IsEqualTo(baseCount + 1);

            await Task.WhenAll(CreateScopeTest(baseCount + 2), CreateScopeTest(baseCount + 3)).ConfigureAwait(false);

            await Assert.That(ScopeValues.Class<ClassSample>.Current?.Value).IsEqualTo(baseCount + 1);
        }

        return;

        Task CreateScopeTest(int value)
        {
            return Task.Run(async () =>
            {
                using (new ClassSample(value).Push())
                {
                    await Assert.That(ScopeValues.Class<ClassSample>.Current?.Value).IsEqualTo(value);
                    await Task.Delay(100).ConfigureAwait(false);
                    await Assert.That(ScopeValues.Class<ClassSample>.Current?.Value).IsEqualTo(value);
                }
            });
        }
    }

    /// <summary>
    /// Value scope test.
    /// </summary>
    /// <param name="count">count.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    [MatrixDataSource]
    public async Task ValueScopeTasksTestAsync(
        [Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        int count)
    {
        var baseCount = count * 10;
        using (new ValueSample(baseCount + 1).Push())
        {
            await Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 1);

            await Task.WhenAll(CreateScopeTest(baseCount + 2), CreateScopeTest(baseCount + 3)).ConfigureAwait(false);

            await Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 1);
        }

        return;

        Task CreateScopeTest(int value)
        {
            return Task.Run(async () =>
            {
                using (new ValueSample(value).Push())
                {
                    await Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(value);
                    await Task.Delay(100).ConfigureAwait(false);
                    await Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(value);
                }
            });
        }
    }

    /// <summary>
    /// Scope base test.
    /// </summary>
    /// <param name="count">count.</param>
    [Test]
    [MatrixDataSource]
#pragma warning disable TUnitAssertions0002
    public void FastScopeTasksTest(
        [Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        int count)
    {
        var baseCount = count * 10;
        using (new ValueSample(baseCount + 1).FastPush())
        {
            Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 1).GetAwaiter().GetResult();

            using (new ValueSample(baseCount + 2).FastPush())
                 Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 2).GetAwaiter().GetResult();

            using (new ValueSample(baseCount + 3).FastPush())
                Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 3).GetAwaiter().GetResult();

            Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 1).GetAwaiter().GetResult();
        }

        Assert.That(ScopeValues.Struct<ValueSample>.HasCurrent).IsFalse().GetAwaiter().GetResult();
    }
#pragma warning restore TUnitAssertions0002
}
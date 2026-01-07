namespace TedToolkit.Scopes.Tests;

internal class ValueScopeTests
{
    [Test]
    [MatrixDataSource]
    public async Task ScopeBaseTasksTest(
        [Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        int count)
    {
        var baseCount = count * 10;
        using (new TestValueScope(baseCount + 1))
        {
            await Assert.That(TestValueScope.Current?.Value).IsEqualTo(baseCount + 1);

            await Task.WhenAll(CreateScopeTest(baseCount + 2), CreateScopeTest(baseCount + 3));

            await Assert.That(TestValueScope.Current?.Value).IsEqualTo(baseCount + 1);
        }

        return;

        Task CreateScopeTest(int value)
        {
            return Task.Run(async () =>
            {
                using (new TestValueScope(value))
                {
                    await Assert.That(TestValueScope.Current?.Value).IsEqualTo(value);
                    await Task.Delay(100);
                    await Assert.That(TestValueScope.Current?.Value).IsEqualTo(value);
                }
            });
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task ValueScopeTasksTest(
        [Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        int count)
    {
        var baseCount = count * 10;
        using (new ValueScope<ValueSample>(new ValueSample(baseCount + 1)))
        {
            await Assert.That(ValueScope<ValueSample>.Current.Value).IsEqualTo(baseCount + 1);

            await Task.WhenAll(CreateScopeTest(baseCount + 2), CreateScopeTest(baseCount + 3));

            await Assert.That(ValueScope<ValueSample>.Current.Value).IsEqualTo(baseCount + 1);
        }

        return;

        Task CreateScopeTest(int value)
        {
            return Task.Run(async () =>
            {
                using (new ValueScope<ValueSample>(new ValueSample(value)))
                {
                    await Assert.That(ValueScope<ValueSample>.Current.Value).IsEqualTo(value);
                    await Task.Delay(100);
                    await Assert.That(ValueScope<ValueSample>.Current.Value).IsEqualTo(value);
                }
            });
        }
    }

    [Test]
    [MatrixDataSource]
#pragma warning disable TUnitAssertions0002
    public void FastScopeTasksTest(
        [Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        int count)
    {
        var baseCount = count * 10;
        using (new FastScope<ValueSample>(new ValueSample(baseCount + 1)))
        {
            Assert.That(FastScope<ValueSample>.Current.Value).IsEqualTo(baseCount + 1).GetAwaiter().GetResult();

            using (new FastScope<ValueSample>(new ValueSample(baseCount + 2)))
            {
                Assert.That(FastScope<ValueSample>.Current.Value).IsEqualTo(baseCount + 2).GetAwaiter().GetResult();
            }

            using (new FastScope<ValueSample>(new ValueSample(baseCount + 3)))
            {
                Assert.That(FastScope<ValueSample>.Current.Value).IsEqualTo(baseCount + 3).GetAwaiter().GetResult();
            }

            Assert.That(FastScope<ValueSample>.Current.Value).IsEqualTo(baseCount + 1).GetAwaiter().GetResult();
        }

        Assert.That(FastScope<ValueSample>.HasCurrent).IsFalse().GetAwaiter().GetResult();
    }
#pragma warning restore TUnitAssertions0002
}
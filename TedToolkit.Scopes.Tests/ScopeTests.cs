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
        using (new ClassSample(baseCount + 1).Push())
        {

            await Assert.That(ScopeValues.Class<ClassSample>.Current?.Value).IsEqualTo(baseCount + 1);

            await Task.WhenAll(CreateScopeTest(baseCount + 2), CreateScopeTest(baseCount + 3));

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
                    await Task.Delay(100);
                    await Assert.That(ScopeValues.Class<ClassSample>.Current?.Value).IsEqualTo(value);
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
        using (new ValueSample(baseCount + 1).Push())
        {
            await Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 1);

            await Task.WhenAll(CreateScopeTest(baseCount + 2), CreateScopeTest(baseCount + 3));

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
                    await Task.Delay(100);
                    await Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(value);
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
        using (new ValueSample(baseCount + 1).FastPush())
        {
            Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 1).GetAwaiter().GetResult();

            using (new ValueSample(baseCount + 2).FastPush())
            {
                Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 2).GetAwaiter().GetResult();
            }

            using (new ValueSample(baseCount + 3).FastPush())
            {
                Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 3).GetAwaiter().GetResult();
            }

            Assert.That(ScopeValues.Struct<ValueSample>.Current.Value).IsEqualTo(baseCount + 1).GetAwaiter().GetResult();
        }

        Assert.That(ScopeValues.Struct<ValueSample>.HasCurrent).IsFalse().GetAwaiter().GetResult();
    }
#pragma warning restore TUnitAssertions0002
}
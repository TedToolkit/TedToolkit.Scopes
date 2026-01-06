namespace TedToolkit.Scopes.Tests;

internal class ScopeTests
{
    [Test]
    [MatrixDataSource]
    public async Task TasksTest(
        [Matrix(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]int count)
    {
        var baseCount = count * 10;
        using (new TestScope(baseCount + 1))
        {
            await Assert.That(TestScope.Current?.Value).IsEqualTo(baseCount + 1);

            await Task.WhenAll(CreateScopeTest(baseCount + 2), CreateScopeTest(baseCount + 3));

            await Assert.That(TestScope.Current?.Value).IsEqualTo(baseCount + 1);
        }

        return;

        Task CreateScopeTest(int value)
        {
            return Task.Run(async () =>
            {
                using (new TestScope(value))
                {
                    await Assert.That(TestScope.Current?.Value).IsEqualTo(value);
                    await Task.Delay(100);
                    await Assert.That(TestScope.Current?.Value).IsEqualTo(value);
                }
            });
        }
    }
}
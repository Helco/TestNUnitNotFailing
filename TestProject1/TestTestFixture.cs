using NUnit.Framework;

namespace TestProject1;

[TestFixture]
[Apartment(ApartmentState.STA), CancelAfter(1000)]
public sealed class TestTestFixture
{
    [SetUp]
    public void Setup()
    {
        TestContext.CurrentContext.CancellationToken.Register(() => Cleanup("Cancellation"));
    }

    [TearDown]
    public void TearDown() => Cleanup("TearDown");

    private static void Cleanup(string reason)
    {
        try
        {
            if (TestContext.CurrentContext.CancellationToken.IsCancellationRequested)
                Assert.Fail("Test was cancelled, most likely due to a timeout.");
        }
        catch (Exception e)
        {
            Assert.Fail($"Exception during {reason}: {e}");
        }
    }

    [Test]
    public async Task T()
    {
        await Task.Delay(30000);
        Assert.Pass();
    }
}

using NUnit.Framework;

namespace TestProject1;

[TestFixture("abc")]
[TestFixture("def")]
[Apartment(ApartmentState.STA), CancelAfter(1000)]
public sealed class TestTestFixture
{
    private readonly string parameter;

    public TestTestFixture(string p) => this.parameter = p;

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
        await Console.Out.WriteLineAsync(parameter);
        await Task.Delay(30000);
        Assert.Fail();
    }
}

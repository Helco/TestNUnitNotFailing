using NUnit.Framework;

namespace TestProject1;

[TestFixture]
[CancelAfter(10000)]
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
    public async Task LoadSynchronousWhileAlreadyStartedToLoad()
    {
        var fileLoader = new TestFileLoader();
        FileLoading.LoadAsync(fileLoader);

        // Make sure that the asset is currently loading
        await fileLoader.WasStarted.Task;
        
        var contents = FileLoading.LoadSync(fileLoader);
        Assert.That(contents, Is.EqualTo("The contents of the loaded file"));
    }
}

public static class FileLoading
{
    public static void LoadAsync(TestFileLoader a) => Task.Run(a.LoadStuff);

    public static string LoadSync(TestFileLoader a)
    {
        // Check that we are currently on the main thread
        // Check whether file was already loaded
        // Check whether file is already being loaded
        // Start and/or wait for completion of loading process
        return "The contents of the loaded file";
    }
}

public class TestFileLoader
{
    // Just to signal for testing that loading has begun
    public TaskCompletionSource WasStarted = new();

    public async Task LoadStuff()
    {
        WasStarted.SetResult();
        await Task.Delay(2000);
    }
}

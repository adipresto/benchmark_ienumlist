using BenchmarkDotNet.Running;

bool cancelRequested = false;

Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true; // mencegah exit langsung
    cancelRequested = true;
    Console.WriteLine("Graceful exit. Cleanup in process...");
};

try
{
    BenchmarkRunner.Run<ForeachBenchmark>();

    if (cancelRequested)
    {
        Console.WriteLine("Benchmark closed by user.");
    }

    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Nice catch!: {ex.Message} | {nameof(ex)}");
    return 1;
}
finally
{
    Console.WriteLine("Cleaning resource, if there are/is...");
}

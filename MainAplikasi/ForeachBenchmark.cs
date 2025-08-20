using BenchmarkDotNet.Attributes;

public class ForeachBenchmark
{
    private List<int> numbers;
    private IEnumerable<int> enumerableNumbers;

    [GlobalSetup]
    public void Setup()
    {
        numbers = Enumerable.Range(0, 1_000_000).ToList();
        enumerableNumbers = Enumerable.Range(0, 1_000_000).ToList();
    }

    [Benchmark]
    public int ForLoop_List()
    {
        int sum = 0;
        for (int i = 0; i < numbers.Count; i++)
            sum += numbers[i];
        return sum;
    }

    [Benchmark]
    public int DIY_Loop_IEnumerable()
    {
        int sum = 0;

        // ambil enumerator
        using var enumerator = enumerableNumbers.GetEnumerator();

        // ambil nilai pertama dari enumerator
        sum = enumerableNumbers.GetEnumerator().Current;

        // mengembalikan boolean kalau ada elemen, selain itu false
        while (enumerator.MoveNext())
        {
            sum += enumerator.Current;
        }

        return sum;
    }

    [Benchmark]
    public int Foreach_List()
    {
        int sum = 0;
        foreach (var n in numbers)
            sum += n;
        return sum;
    }

    [Benchmark]
    public int Foreach_IEnumerable()
    {
        int sum = 0;
        foreach (var n in enumerableNumbers)
            sum += n;
        return sum;
    }
}

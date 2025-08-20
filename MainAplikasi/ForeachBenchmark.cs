using BenchmarkDotNet.Attributes;

public class ForeachBenchmark
{
    private int[] array;
    private List<int> numbers;
    private IEnumerable<int> enumerableNumbers;
    private LinkedList<int> linkedList;

    [GlobalSetup]
    public void Setup()
    {
        array = Enumerable.Range(0, 1_000_000).ToArray();
        numbers = Enumerable.Range(0, 1_000_000).ToList();
        enumerableNumbers = Enumerable.Range(0, 1_000_000).ToList();
        linkedList = new LinkedList<int>(Enumerable.Range(0, 1_000_000));
    }

    [Benchmark]
    public int ForLoop_List()
    {
        int sum = 0;

        // index-bounding cukup sekali ambil
        int count = numbers.Count;

        for (int i = 0; i < count; i++)
            sum += numbers[i];
        return sum;
    }

    [Benchmark]
    public int DIY_Loop_IEnumerable()
    {
        int sum = 0;

        // ambil enumerator
        using var enumerator = enumerableNumbers.GetEnumerator();

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

    [Benchmark]
    public int Foreach_LinkedList()
    {
        int sum = 0;
        foreach (var n in linkedList)
            sum += n;
        return sum;
    }

    [Benchmark]
    public int Enumerator_LinkedList()
    {
        int sum = 0;
        using var enumerator = linkedList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            sum += enumerator.Current;
        }
        return sum;
    }

    [Benchmark]
    public int ForLoop_LinkedList()
    {
        // Jangan pakai for-loop dengan indeks: LinkedList tidak support O(1) index
        int sum = 0;
        var node = linkedList.First;
        while (node != null)
        {
            sum += node.Value;
            node = node.Next;
        }
        return sum;
    }

    [Benchmark]
    public int ForLoop_Array()
    {
        int sum = 0;
        int length = array.Length; // cache length untuk optimalisasi
        for (int i = 0; i < length; i++)
            sum += array[i];
        return sum;
    }

    [Benchmark]
    public int Foreach_Array()
    {
        int sum = 0;
        foreach (var n in array)
            sum += n;
        return sum;
    }
}

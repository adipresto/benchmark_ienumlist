# Which(true) one? 
Yuk kita kulik, mana yang lebih cepat untuk iterasi struktur data koleksi seperti `List<T>` dan kakeknya `IEnumerable<T>`. Kudos mas [](url)

## Tujuan akhir
Untuk tau mean waktu operasi iterasi yang paling optimal dan jenis struktur koleksi data yang mana yang baik

# Spesifikasi perangkat benchmark
Ini kekurangannya karena saya pakai device pribadi saya sendiri. Untuk lebih ciamik kalau tes ini dilakukan pada mesin yang lemot luar biasa

<img width="665" height="91" alt="image" src="https://github.com/user-attachments/assets/ed72d8b3-6d16-4d0b-9242-7629f0654f7b" />

# Hasil Benchmark

| Method                     | Mean (μs) | Catatan |
|---------------------------- |-----------|---------|
| ForeachLoop_List_toSpan     | 394.5     | Paling cepat untuk List besar (via Span) |
| Foreach_Array               | 397.6     | Hampir sama cepat, pointer-based loop oleh JIT |
| ForLoop_List_toSpan         | 545.8     | For-loop List + Span, menghilangkan bounds-checking |
| ForLoop_Array               | 569.0     | For-loop array biasa, cache friendly |
| ForLoop_List                | 668.9     | For-loop biasa dengan `Count` cached |
| Foreach_List                | 855.7     | Standard foreach, enumerator overhead |

> IEnumerable<T> bisa dilakukan pemeriksaan secara mandiri. Yang jelas, dengan spesifikasi perangkat benchmark yang ku miliki didapati dengan mean 5,5k - 6k μs

## Hikmah yang dipetik

Iterasi `for` dengan **pemilihan logika yang tepat** untuk `List<T>` adalah jalan paling optimal.  
Secara belakang layar, `foreach` pada `List<T>` **dioptimalkan menjadi `for` loop** dengan `count` yang didefinisikan terlebih dahulu untuk mengurangi overhead bound-checking.  

Untuk array, `foreach` juga sangat cepat karena **pointer-based iteration** yang dioptimalkan oleh JIT compiler.

## Strategi Iterasi
1. `For`

Untuk `List<T>` terlebih dulu ambil `count` untuk mengurangi <i>overhead</i> saat inbound-check
```cs

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
```

Dan untuk `IEnumerable<T>` karena tidak bisa dilakukan operasi iterasi `for` kita lewati saja dengan menggunakan `Foreach`

2. `Foreach`

Baik `List<T>` atau `IEnumerable<T>` diberlakukan dengan seimbang
```cs

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
```

3. `While`
Untuk menanggulangi ketidak efektifan dari iterasi `foreach` untuk `IEnumerable<T>`, saya membuat sendiri lojik iterasi untuk berusaha mengurangi <i>overhead</> yang terhadi selama operasi `foreach`

```cs
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
```



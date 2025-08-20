# Which(true) one? 
Yuk kita kulik, mana yang lebih cepat untuk iterasi struktur data koleksi seperti `List<T>` dan kakeknya `IEnumerable<T>`. Kudos mas [](url)

## Tujuan akhir
Untuk tau mean waktu operasi iterasi yang paling optimal dan jenis struktur koleksi data yang mana yang baik

# Spesifikasi perangkat benchmark
Ini kekurangannya karena saya pakai device pribadi saya sendiri. Untuk lebih ciamik kalau tes ini dilakukan pada mesin yang lemot luar biasa

<img width="665" height="91" alt="image" src="https://github.com/user-attachments/assets/ed72d8b3-6d16-4d0b-9242-7629f0654f7b" />

# Hasilnya

<img width="503" height="113" alt="image" src="https://github.com/user-attachments/assets/4031d086-5ced-4768-ab92-6ee73dba9b7a" />

## Hikmah yang dipetik

Iterasi `for` dan dengan pemilihan lojik yang tepat untuk `List<T>` adalah jalan paling optimal. Secara dibelakang layar yang terjadi pada `foreach` untuk `List<T>` adalah `for` dengan `count` yang didefinisikan terlebih dulu untuk mengoptimalkan operasi bound-checking

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



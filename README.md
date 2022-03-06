# Task vs ValueTask

Comparison between `Task` and `ValueTask` in C#.

If your code's hot path is synchronous in most of the cases then you can use `ValueTask` to reduce some memory allocation. But if your code's hot path is always asynchronous then `Task` should be used. Because in that case we will not get any performance benefit by using `ValueTask` over `Task`.

Benchmark comparison  result of the code which is always asynchronous.

```
// * Summary *

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Xeon Platinum 8272CL CPU 2.60GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.200
  [Host]     : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  DefaultJob : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT

|            Method |    Mean |    Error |   StdDev |  Median | Allocated |
|------------------ |--------:|---------:|---------:|--------:|----------:|
|      ReadWithTask | 1.171 s | 0.0325 s | 0.0884 s | 1.215 s |    205 KB |
| ReadWithValueTask | 1.191 s | 0.0248 s | 0.0654 s | 1.219 s |    205 KB |
```

Benchmark comparison result of the code which is 99% of the times synchronous.

```
// * Summary *

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Xeon Platinum 8272CL CPU 2.60GHz, 1 CPU, 2 logical and 2 physical cores
.NET SDK=6.0.200
  [Host]     : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  DefaultJob : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT

|            Method |    Mean |    Error |   StdDev |       Gen 0 | Allocated |
|------------------ |--------:|---------:|---------:|------------:|----------:|
|      ReadWithTask | 4.123 s | 0.0511 s | 0.0478 s | 513000.0000 |      9 GB |
| ReadWithValueTask | 3.738 s | 0.0167 s | 0.0157 s | 128000.0000 |      2 GB |
```

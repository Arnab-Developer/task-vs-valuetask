using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

/*

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

*/

[MemoryDiagnoser]
public class HttpReader
{
    private const string _targetUrl = "https://google.com";

    [Benchmark]
    public async Task<HttpContent> ReadWithTask()
    {
        using HttpClient client = new();
        HttpResponseMessage responseMessage = await client.GetAsync(_targetUrl);
        return responseMessage.Content;
    }

    [Benchmark]
    public async ValueTask<HttpContent> ReadWithValueTask()
    {
        using HttpClient client = new();
        HttpResponseMessage responseMessage = await client.GetAsync(_targetUrl);
        return responseMessage.Content;
    }
}
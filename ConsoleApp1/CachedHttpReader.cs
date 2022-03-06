using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

/*

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

*/

[MemoryDiagnoser]
public class HttpService
{
    private const int _maxCount = 100000000;

    [Benchmark]
    public async Task<HttpContent?> ReadWithTask()
    {
        HttpContent? httpContent = null;
        for (int counter = 1; counter <= _maxCount; counter++)
        {
            CachedHttpReader cachedHttpReader = new();
            httpContent = await cachedHttpReader.ReadWithTask();
        }
        return httpContent;
    }

    [Benchmark]
    public async ValueTask<HttpContent?> ReadWithValueTask()
    {
        HttpContent? httpContent = null;
        for (int counter = 1; counter <= _maxCount; counter++)
        {
            CachedHttpReader cachedHttpReader = new();
            httpContent = await cachedHttpReader.ReadWithValueTask();
        }
        return httpContent;
    }
}

internal class CachedHttpReader
{
    private const string _targetUrl = "https://google.com";

    private static HttpResponseMessage? s_ResponseTask;
    private static HttpResponseMessage? s_ResponseValueTask;    
        
    public async Task<HttpContent> ReadWithTask()
    {
        if (s_ResponseTask == null)
        {
            using HttpClient client = new();
            s_ResponseTask = await client.GetAsync(_targetUrl);
        }
        return s_ResponseTask.Content;
    }

    public async ValueTask<HttpContent> ReadWithValueTask()
    {
        if (s_ResponseValueTask == null)
        {
            using HttpClient client = new();
            s_ResponseValueTask = await client.GetAsync(_targetUrl);
        }
        return s_ResponseValueTask.Content;
    }
}
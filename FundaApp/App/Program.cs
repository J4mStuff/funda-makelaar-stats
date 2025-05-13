using System.Collections.Generic;
using System.Threading.Tasks;
using Services;

namespace App;

public static class Program
{
    public static void Main()
    {
        Run().Wait();
    }

    private static async Task Run()
    {
        var httpClient = new HttpClientWrapper();
        var tasks = new List<Task>
        {
            StatRequestProcessor.ProcessStats(httpClient, true),
            StatRequestProcessor.ProcessStats(httpClient, false)
        };
        
        await Task.WhenAll(tasks);
    }
}
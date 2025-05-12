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
        await StatRequestProcessor.ProcessStats(true);
        await StatRequestProcessor.ProcessStats(false);
    }
}
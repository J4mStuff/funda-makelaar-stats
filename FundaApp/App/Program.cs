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
        var handler = new RequestHandler();
        await handler.GetAsync();
    }
}
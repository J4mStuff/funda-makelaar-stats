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
        RequestHandler requestHandler = new();
        AgentStatProcessor agentStatProcessor = new(); 
        
        var entriesWithNoGarden = await requestHandler.MakeRequests(false);
        AgentStatProcessor.ProcessAgents(entriesWithNoGarden);
        
        var entriesWithGarden = await requestHandler.MakeRequests(true);
        AgentStatProcessor.ProcessAgents(entriesWithGarden);
    }
}
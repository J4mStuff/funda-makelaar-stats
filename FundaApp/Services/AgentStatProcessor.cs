using Models;

namespace Services;

public class AgentStatProcessor
{
    public static void ProcessAgents(ICollection<Entry> entries)
    {
        var statEntries = new List<StatEntry>();
        var x = entries.GroupBy(e => e.AgentId).OrderBy(g => g.Count()).Take(10).ToList();
        var y = x.ToDictionary(g => g.Key, g => g.ToList());

        foreach (var (_, agentEntries) in y)
        {
            var first = agentEntries.First();
            var statusGroups = agentEntries.GroupBy(g => g.Status).ToDictionary(g => g.Key, g => g.Count());

            var statEntry = new StatEntry
            {
                AgentId = first.AgentId,
                AgentName = first.AgentName,
                StatusCount = statusGroups,
                TotalCount = agentEntries.Count,
            };
            statEntries.Add(statEntry);
        }
        
        Console.WriteLine(x.Count());
    }
}
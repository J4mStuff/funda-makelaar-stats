using Models;

namespace Services;

public static class AgentStatProcessor
{
    public static List<StatEntry> ProcessAgents(List<Entry> entries)
    {
        var statEntries = new List<StatEntry>();

        var groupedEntries = entries
            .GroupBy(entry => entry.AgentId)
            .OrderByDescending(group => group.Count())
            .Take(10)
            .ToList();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var agentEntries in groupedEntries)
        {
            var first = agentEntries.First();

            var statEntry = new StatEntry
            {
                AgentId = first.AgentId,
                AgentName = first.AgentName,
                TotalCount = agentEntries.Count(),
            };
            statEntries.Add(statEntry);
        }

        return statEntries;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services;
using Xunit;

namespace ServicesTests;

public class AgentStatProcessorTests
{
    [Fact]
    public void ProcessAgents_WhenCalled_ReturnsExpected()
    {
        var entries = BuildEntryList(100);
        var result = AgentStatProcessor.ProcessAgents(entries);

        Assert.Equal(10, result.Count);
        Assert.Equal(10, result.First().TotalCount);
    }

    [Fact]
    public void ProcessAgents_WhenListEmpty_ReturnsEmpty()
    {
        var entries = new List<Entry>();
        var result = AgentStatProcessor.ProcessAgents(entries);

        Assert.Empty(result);
    }

    [Fact]
    public void ProcessAgents_WhenListBelowTen_ReturnsExpected()
    {
        var entries = BuildEntryList(3);
        var result = AgentStatProcessor.ProcessAgents(entries);

        Assert.Single(result);
        Assert.Equal(3, result.First().TotalCount);
    }

    [Fact]
    public void ProcessAgents_WhenListAboveTenButLessThanTenAgents_ReturnsExpected()
    {
        var entries = BuildEntryList(20);
        var result = AgentStatProcessor.ProcessAgents(entries);

        Assert.Equal(3, result.Count);
        Assert.Equal(10, result.First().TotalCount);
    }

    private static List<Entry> BuildSingleAgentEntry(int agentId, int entryCount)
    {
        var entries = new List<Entry>();
        for (var i = 0; i < entryCount; i++)
        {
            entries.Add(new Entry
            {
                AgentId = agentId,
                AgentName = $"agent {agentId}",
                Id = Guid.NewGuid()
            });
        }

        return entries;
    }

    private static List<Entry> BuildEntryList(int entryCount)
    {
        var entries = new List<Entry>();
        var countAdded = 0;

        for (var i = 10; i >= 0 || countAdded >= entryCount; i--)
        {
            var toAdd = countAdded + i >= entryCount ? entryCount - countAdded : i;

            if (toAdd == 0)
            {
                break;
            }

            entries.AddRange(BuildSingleAgentEntry(10 - i, toAdd));
            countAdded += toAdd;
        }

        for (var i = countAdded; i < entryCount; i++)
        {
            entries.Add(new Entry
            {
                AgentId = i,
                AgentName = $"agent {i}",
                Id = Guid.NewGuid()
            });
        }

        return entries;
    }
}
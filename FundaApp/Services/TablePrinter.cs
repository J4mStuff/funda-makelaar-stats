using ConsoleTables;
using Models;

namespace Services;

public static class TablePrinter
{
    public static void PrintListToTable(ICollection<StatEntry> statEntries, bool gardenPresent)
    {
        Logger.Info($"Data for <GardenPresent={gardenPresent}>");
        ConsoleTable.From(statEntries).Write();
    }
}
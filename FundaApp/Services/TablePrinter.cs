using ConsoleTables;
using Models;

namespace Services;

public static class TablePrinter
{
    public static void PrintListToTable(ICollection<StatEntry> statEntries)
    {
        ConsoleTable.From(statEntries).Write();
    }
}
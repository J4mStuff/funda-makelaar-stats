using System.Threading.Tasks;
using Services;
using Xunit;

namespace ServicesTests;

public class EntryListBuilderTests
{
    private readonly EntryListBuilder _underTest;

    [Fact]
    public async Task BuildEntryList_WhenCalled_CallsExpected()
    {
        
        var result = _underTest.BuildEntryList(false);
    }
}
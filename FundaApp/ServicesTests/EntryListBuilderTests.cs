using System;
using System.Threading.Tasks;
using Models;
using NSubstitute;
using Services;
using Xunit;

namespace ServicesTests;

public class EntryListBuilderTests
{
    private readonly EntryListBuilder _underTest;
    private readonly IDataRetriever _dataRetriever;

    public EntryListBuilderTests()
    {
        _dataRetriever = Substitute.For<IDataRetriever>();
        _underTest = new EntryListBuilder(_dataRetriever);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task BuildEntryList_WhenCalled_CallsExpected(bool garden)
    {
        var responseModel = new ResponseModel
        {
            EntryCountTotal = 1,
            Objects =
            [
                new Entry
                {
                    AgentId = 1,
                    AgentName = "Agent 1",
                    Id = Guid.NewGuid()
                }
            ],
            Paging = new Paging
            {
                CurrentPage = 1,
                PageCount = 5
            }
        };

        const string uri = "someUri";
        _dataRetriever.BuildRequestUri(garden).Returns(uri);
        _dataRetriever.RetrievePageData(uri, Arg.Any<int>()).Returns(responseModel);


        var result = await _underTest.BuildEntryList(garden);

        await _dataRetriever.Received(5).RetrievePageData(uri, Arg.Any<int>());
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task BuildEntryList_WhenOnePageOnly_CallsRetrievePageDataOnce()
    {
        var responseModel = new ResponseModel
        {
            EntryCountTotal = 1,
            Objects =
            [
                new Entry
                {
                    AgentId = 1,
                    AgentName = "Agent 1",
                    Id = Guid.NewGuid()
                }
            ],
            Paging = new Paging
            {
                CurrentPage = 1,
                PageCount = 1
            }
        };

        const string uri = "someUri";
        _dataRetriever.BuildRequestUri(false).Returns(uri);
        _dataRetriever.RetrievePageData(uri, 1).Returns(responseModel);


        var result = await _underTest.BuildEntryList(false);

        await _dataRetriever.Received(1).RetrievePageData(uri, 1);
        Assert.Single(result);
    }
}
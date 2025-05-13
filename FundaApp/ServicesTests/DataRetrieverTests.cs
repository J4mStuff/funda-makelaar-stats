using System;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using NSubstitute;
using Services;
using Xunit;

namespace ServicesTests;

public class DataRetrieverTests
{
    private readonly DataRetriever _underTest;
    private readonly IHttpClientWrapper _client = Substitute.For<IHttpClientWrapper>();

    public DataRetrieverTests()
    {
        _underTest = new DataRetriever(_client, "someApiKey");
    }

    [Fact]
    public async Task RetrievePageData_WhenValidResponse_ReturnsExpected()
    {
        const string unpaginatedUri = "http://example.com/";
        var expectedModel = BuildSampleResponseString();

        //This tried to actually do things rather than mock response, not sure why
        _client.GetAndEnsureSuccessAsync(Arg.Any<string>()).Returns(JsonConvert.SerializeObject(expectedModel));

        var result = await _underTest.RetrievePageData(unpaginatedUri, 1);

        Assert.Equal(expectedModel.EntryCountTotal, result.EntryCountTotal);
    }

    [Fact]
    public async Task RetrievePageData_WhenResponseStringEmpty_ThrowsNullException()
    {
        const string unpaginatedUri = "http://example.com/";

        //This tried to actually do things rather than mock response, not sure why
        _client.GetAndEnsureSuccessAsync(Arg.Any<string>()).Returns(string.Empty);

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _underTest.RetrievePageData(unpaginatedUri, 1));
    }

    [Theory]
    [InlineData(true, "json/someApiKey/?type=koop&zo=/amsterdam/tuin/")]
    [InlineData(false, "json/someApiKey/?type=koop&zo=/amsterdam/")]
    public void BuildRequestUri_ReturnsExpected(bool withGarden, string expectedString)
    {
        var response = _underTest.BuildRequestUri(withGarden);
        Assert.Equal(expectedString, response);
    }

    private static ResponseModel BuildSampleResponseString()
    {
        return new ResponseModel
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
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NSubstitute;
using Services;
using Xunit;

namespace ServicesTests;

public class DataRetrieverTests
{
    private readonly DataRetriever _underTest;
    private readonly HttpClient _client = Substitute.For<HttpClient>();
    
    public DataRetrieverTests()
    {
        _underTest = new DataRetriever(_client, "someApiKey");
    }

    // [Fact]
    // public async Task RetrievePageData_WhenValidResponse_ReturnsExpected()
    // {
    //     const string unpaginatedUri = "http://example.com/";
    
    //     //This tried to actually do things rather than mock response, not sure why
    //     _client.GetAsync(Arg.Any<Uri>()).Returns(new HttpResponseMessage(HttpStatusCode.OK));
    //     
    //     var result = await _underTest.RetrievePageData(unpaginatedUri, 1);
    //     
    //     Assert.NotNull(result);
    // }
    //
    // [Fact]
    // public async Task RetrievePageData_WhenRequestFailed_Throws()
    // {
    //     
    // }
    //
    // [Fact]
    // public async Task RetrievePageData_WhenSerializationFails_ThrowsNullException()
    // {
    //     
    // }

    [Theory]
    [InlineData(true, "json/someApiKey/?type=koop&zo=/amsterdam/tuin/")]
    [InlineData(false,"json/someApiKey/?type=koop&zo=/amsterdam/")]
    public void BuildRequestUri_ReturnsExpected(bool withGarden, string expectedString)
    {
        const string uri = "someUri";
        _client.BaseAddress.Returns(new Uri(uri));
        var response = _underTest.BuildRequestUri(withGarden);
        
        Assert.Equal(uri+expectedString, response);
    }
}
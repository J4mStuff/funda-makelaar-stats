using Models;
using Newtonsoft.Json;

namespace Services;

public class DataRetriever(IHttpClientWrapper httpClient, string apiKey) : IDataRetriever
{
    private readonly JsonSerializer _jsonSerializer = JsonSerializer.Create();

    public async Task<ResponseModel> RetrievePageData(string unpaginatedUri, int pageNumber)
    {
        /*
         There's a bug / "feature" where requesting page size > 25 returns 25 items per page,
         but total page count assumes page count with the requested size
         for example, there's 4583 objects total in this endpoint, asking for 500 items per page
         returns 25 items per page, but only 10 pages total in the response body.

         I understand the limitation, but it should:
         > return a bad request
         or
         > properly calculate page total
         */
        var uri = $"{unpaginatedUri}&page={pageNumber}&pagesize=25";
        var jsonResponse = await httpClient.MakeRateLimitedRequest(uri);
        return ParseResponse(jsonResponse);
    }

    private ResponseModel ParseResponse(string jsonResponse)
    {
        var responseObject =
            _jsonSerializer.Deserialize<ResponseModel>(new JsonTextReader(new StringReader(jsonResponse)));
        ArgumentNullException.ThrowIfNull(responseObject);
        return responseObject;
    }

    public string BuildRequestUri(bool withGarden)
    {
        var url = $"json/{apiKey}/?type=koop&zo=/amsterdam/";
        return withGarden ? $"{url}tuin/" : url;
    }
}
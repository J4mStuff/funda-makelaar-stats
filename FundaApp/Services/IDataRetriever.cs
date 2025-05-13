using Models;

namespace Services;

public interface IDataRetriever
{
    public Task<ResponseModel> RetrievePageData(string unpaginatedUri, int pageNumber);
    public string BuildRequestUri(bool withGarden);
}
namespace Services;

public interface IHttpClientWrapper
{
    public Task<string> GetAndEnsureSuccessAsync(string uri);
}
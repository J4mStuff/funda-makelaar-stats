namespace Services;

public interface IHttpClientWrapper
{
    public Task<string> MakeRateLimitedRequest(string uri);
}
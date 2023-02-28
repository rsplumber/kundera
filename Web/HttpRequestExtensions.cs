namespace Web;

public static class HttpRequestExtensions
{
    public static string UserAgent(this HttpRequest request)
    {
        var agentHeader = request.Headers["User-Agent"];
        return agentHeader.Count > 0 ? agentHeader[0] ?? string.Empty : string.Empty;
    }
}
namespace Web.Api;

internal sealed class CustomErrorResponse
{
    public string Message { get; init; } = string.Empty;

    public IList<string> Errors { get; init; } = Array.Empty<string>();
}
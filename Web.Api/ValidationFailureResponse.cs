namespace Web.Api;

public class ValidationFailureResponse
{
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();
}
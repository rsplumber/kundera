namespace Data.Abstractions;

public record PageableQuery
{
    public int Page { get; set; } = 1;

    public int Size { get; set; } = 10;
}

public record PageableResponse<TItem>
{
    public int TotalPages { get; set; }

    public int TotalItems { get; set; }

    public List<TItem> Data { get; set; } = new();
}
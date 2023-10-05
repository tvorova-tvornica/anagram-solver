namespace AnagramSolver.Controllers.Celebrities.Dto;

public record SolveAnagramResult
{
    public required string FullName { get; init; }
    public string? PhotoUrl { get; init; }
    public string? Description{ get; init; }
    public string? WikipediaUrl { get; init; }
}

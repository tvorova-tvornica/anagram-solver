namespace AnagramSolver.Controllers.Dto;

public record ResolveAnagramResult
{
    public string? FullName { get; set; }
    public string? PhotoUrl { get; set; }
    public string? WikipediaUrl { get; set; }
}

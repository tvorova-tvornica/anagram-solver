namespace AnagramSolver.Controllers.Dto;

public record ResolveAnagramResult
{
    public required string FullName { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Description{ get; set; }
    public string? WikipediaUrl { get; set; }
}

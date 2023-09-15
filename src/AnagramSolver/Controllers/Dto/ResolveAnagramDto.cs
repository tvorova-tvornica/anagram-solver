using System.ComponentModel.DataAnnotations;
using AnagramSolver.Extensions;

namespace AnagramSolver.Controllers.Dto;

public record ResolveAnagramDto
{
    [Required]
    public required string Anagram { get; init; }
    public string? AnagramKey => Anagram?.ToAnagramKey();
}

using System.ComponentModel.DataAnnotations;
using AnagramSolver.Extensions;

namespace AnagramSolver.Controllers.Celebrities.Dto;

public record SolveAnagramDto
{
    [Required]
    public required string Anagram { get; init; }
    public string? AnagramKey => Anagram?.ToAnagramKey();
}

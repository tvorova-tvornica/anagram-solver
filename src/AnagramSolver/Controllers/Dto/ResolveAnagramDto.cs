using AnagramSolver.Extensions;

namespace AnagramSolver.Controllers.Dto;

public record ResolveAnagramDto
{
    public required string Anagram { get; init; }
    public string SortedAnagram => Anagram.ToSortedLowercaseWithoutWhitespaces();
}

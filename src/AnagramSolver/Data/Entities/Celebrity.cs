using AnagramSolver.Exceptions;
using AnagramSolver.Extensions;

namespace AnagramSolver.Data.Entities;

public class Celebrity 
{
    public int Id { get; private set; }
    public string FullName { get; private init; }
    public string AnagramKey { get; private init; }
    public string? PhotoUrl { get; init; }
    public string? WikipediaUrl { get; init; }
    public string? Description { get; init; }

    public Celebrity(string fullName)
    {
        var nameWithoutWhitespacesAndPunctuation = fullName.ToRemovedWhitespace().ToRemovedPunctuation();

        if (string.IsNullOrWhiteSpace(nameWithoutWhitespacesAndPunctuation))
        {
            throw new BusinessRuleViolationException($"Celebrity full name must contain letters: {fullName}");
        }

        FullName = fullName;
        AnagramKey = fullName.ToAnagramKey();
    }
}

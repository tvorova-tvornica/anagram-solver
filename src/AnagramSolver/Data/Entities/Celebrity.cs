using AnagramSolver.Exceptions;
using AnagramSolver.Extensions;

namespace AnagramSolver.Data.Entities;

public class Celebrity 
{
    public int Id { get; private set; }
    public string FullName { get; private set; }
    public string AnagramKey { get; private set; }
    public string? HrFullName { get; private set; }
    public string? HrAnagramKey { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string? WikipediaUrl { get; private set; }
    public string? Description { get; private set; }
    public string? WikiDataPageId { get; init; }

    public Celebrity(string fullName, 
                     string? hrFullName = null, 
                     string? photoUrl = null,
                     string? description = null,
                     string? wikipediaUrl = null)
    {
        ValidateFullName(fullName);

        FullName = fullName;
        AnagramKey = fullName.ToAnagramKey();
        SetHrFullName(fullName, hrFullName);
        PhotoUrl = photoUrl;
        Description = description;
        WikipediaUrl = wikipediaUrl;
    }

    public void Update(string fullName, 
                       string? hrFullName, 
                       string? photoUrl,
                       string? description,
                       string? wikipediaUrl)
    {
        ValidateFullName(fullName);

        this.FullName = fullName;
        this.AnagramKey = fullName.ToAnagramKey();
        SetHrFullName(fullName, hrFullName);
        this.PhotoUrl = photoUrl;
        this.Description = description;
        this.WikipediaUrl = wikipediaUrl;
    }

    private void SetHrFullName(string fullName, string? hrFullName)
    {
        var hasNonNullUniqueHrName = hrFullName?.ToRemovedWhitespace().ToRemovedPunctuation() is not null &&
                    !string.Equals(fullName, hrFullName, StringComparison.OrdinalIgnoreCase);

        if (hasNonNullUniqueHrName)
        {
            HrFullName = hrFullName;
            HrAnagramKey = hrFullName?.ToAnagramKey();
        }
        else 
        {
            HrFullName = null;
            HrAnagramKey = null;
        }
    }

    private void ValidateFullName(string fullName)
    {
        var nameWithoutWhitespacesAndPunctuation = fullName.ToRemovedWhitespace().ToRemovedPunctuation();

        if (string.IsNullOrWhiteSpace(nameWithoutWhitespacesAndPunctuation))
        {
            throw new BusinessRuleViolationException($"Celebrity full name must contain letters: {fullName}");
        }
    }
}

using AnagramSolver.Extensions;

namespace AnagramSolver.Data.Entities;

public class Celebrity 
{
    public int Id { get; private set; }
    public string FullName { get; init; }
    public string AnagramKey { get; init; }

    public Celebrity(string fullName)
    {
        var nameWithoutWhitespacesAndPunctuation = fullName.ToRemovedWhitespace().ToRemovedPunctuation();

        if (string.IsNullOrWhiteSpace(nameWithoutWhitespacesAndPunctuation))
        {
            throw new InvalidFullNameException("Name must contain letters");
        }

        FullName = fullName;
        AnagramKey = fullName.ToAnagramKey();
    }
}

namespace AnagramSolver.Extensions;

public static class StringExtensions
{
    public static string ToSortedLowercaseWithoutWhitespaces(this string value)
    {
        return new string(value.ToLower().Where(x => !char.IsWhiteSpace(x)).OrderBy(x => x).ToArray());
    }
}
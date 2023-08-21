namespace AnagramSolver.Extensions;

public static class StringExtensions
{
    public static string ToTrimmedSorted(this string value)
    {
        return new string(value.Where(x => !char.IsWhiteSpace(x)).OrderBy(x => x).ToArray());
    }
}
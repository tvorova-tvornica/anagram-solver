using System.Globalization;
using System.Text;

namespace AnagramSolver.Extensions;

public static class StringExtensions
{
    public static string ToAnagramKey(this string value)
    {
        return value.Normalize(NormalizationForm.FormKC)
                    .ToUpperInvariant()
                    .ToRemovedWhitespace()
                    .ToRemovedPunctuation()
                    .ToOrderedByOrdinalValue();
    }

    public static string ToRemovedWhitespace(this string value)
    {
        return new string(value.Where(x => !char.IsWhiteSpace(x)).ToArray());
    }

    public static string ToRemovedPunctuation(this string value)
    {
        var sb = new StringBuilder();

        foreach (var rune in value.EnumerateRunes())
            if (!Rune.IsPunctuation(rune))
                sb.Append(rune);
                
        return sb.ToString();
    }

    public static string ToOrderedByOrdinalValue(this string value)
    {
        var graphemeClusters = new List<string>();
        var textElements = StringInfo.GetTextElementEnumerator(value);

        while (textElements.MoveNext())
            graphemeClusters.Add(textElements.Current.ToString()!);

        return string.Concat(graphemeClusters.OrderBy(x => x, StringComparer.Ordinal).ToList());
    }
}

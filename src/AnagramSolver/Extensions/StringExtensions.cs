using System.Globalization;
using System.Text;

namespace AnagramSolver.Extensions;

public static class StringExtensions
{
    public static string ToAnagramKey(this string value)
    {
        return value.ToLower()
                    .ToRemovedWhitespace()
                    .ToRemovedPunctuation()
                    .ToAlphabeticallyOrdered();
    }

    public static string ToRemovedWhitespace(this string value)
    {
        return new string(value.Where(x => !char.IsWhiteSpace(x)).ToArray());
    }

    public static string ToRemovedPunctuation(this string value)
    {
        return new string(value.Where(x => !char.IsPunctuation(x)).ToArray());
    }

    public static string ToAlphabeticallyOrdered(this string value)
    {
        var enumerator = StringInfo.GetTextElementEnumerator(value);
        var elements = new StringBuilder();

        while (enumerator.MoveNext())
        {
            elements.Append(enumerator.GetTextElement());
        }

        string normalizedString = elements.ToString().Normalize(NormalizationForm.FormD);

        char[] sortedCharacters = normalizedString.ToCharArray();
        Array.Sort(sortedCharacters);

        return new string(sortedCharacters);
        //return new string(value.OrderBy(x => x).ToArray());
    }
}

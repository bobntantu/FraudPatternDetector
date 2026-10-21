
using FraudPatternDetector.Models;

namespace FraudPatternDetector.Algorithms;

public sealed class NaiveStringMatcher : ISubstringMatcher
{
    public StringMatchingAlgorithm Algorithm =>
        StringMatchingAlgorithm.Naive;

    public IReadOnlyList<int> FindAll(
        string text,
        string pattern,
        SearchMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(pattern);
        ArgumentNullException.ThrowIfNull(metrics);

        var matches = new List<int>();

        if (pattern.Length == 0 || pattern.Length > text.Length)
        {
            return matches;
        }

        for (int i = 0; i <= text.Length - pattern.Length; i++)
        {
            bool found = true;

            for (int j = 0; j < pattern.Length; j++)
            {
                metrics.CharacterComparisons++;

                if (text[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                matches.Add(i);
            }
        }

        return matches;
    }
}

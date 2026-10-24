

using FraudPatternDetector.Models;

namespace FraudPatternDetector.Algorithms;

// Class that uses Naive String Matching algorithm to find pattern in text
public sealed class NaiveStringMatcher : ISubstringMatcher
{
    // Return name of the algorithm 
    public StringMatchingAlgorithm Algorithm =>
        StringMatchingAlgorithm.Naive;

    public IReadOnlyList<int> FindAll(
        string text,
        string pattern,
        SearchMetrics metrics)
    {
        // Check inputs before starting search
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(pattern);
        ArgumentNullException.ThrowIfNull(metrics);

        // Store the starting position
        var matches = new List<int>();

        // No match if pattern is empty or longer than text
        if (pattern.Length == 0 || pattern.Length > text.Length)
        {
            return matches;
        }

        // Move through text
        for (int i = 0; i <= text.Length - pattern.Length; i++)
        {
            
            bool found = true;

            // Compare each character of pattern
            for (int j = 0; j < pattern.Length; j++)
            {
                // Count each character comparison
                metrics.CharacterComparisons++;

                // If characters are different, not a match.
                if (text[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }

            // If all characters match save the position
            if (found)
            {
                matches.Add(i);
            }
        }

        // Return all matched positions
        return matches;
    }
}

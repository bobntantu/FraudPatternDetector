
using FraudPatternDetector.Models;

namespace FraudPatternDetector.Algorithms;

public sealed class RabinKarpMatcher : ISubstringMatcher
{
    private const long Base = 256;
    private const long Modulus = 1_000_000_007;

    public StringMatchingAlgorithm Algorithm =>
        StringMatchingAlgorithm.RabinKarp;

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

        int patternLength = pattern.Length;
        int textLength = text.Length;

        long patternHash = 0;
        long windowHash = 0;
        long highestPower = 1;

        // Calculate Base^(patternLength - 1) modulo Modulus.
        for (int i = 1; i < patternLength; i++)
        {
            highestPower = (highestPower * Base) % Modulus;
        }

        // Calculate the initial pattern and text-window hashes.
        for (int i = 0; i < patternLength; i++)
        {
            patternHash = (
                patternHash * Base + pattern[i]
            ) % Modulus;

            windowHash = (
                windowHash * Base + text[i]
            ) % Modulus;
        }

        metrics.HashCalculations += 2;

        for (int i = 0; i <= textLength - patternLength; i++)
        {
            if (patternHash == windowHash)
            {
                metrics.HashHitsVerified++;

                if (VerifyMatch(text, pattern, i, metrics))
                {
                    matches.Add(i);
                }
            }

            // Calculate the next window's hash using the
            // previous window's hash.
            if (i < textLength - patternLength)
            {
                long outgoing = (
                    text[i] * highestPower
                ) % Modulus;

                windowHash = (
                    windowHash - outgoing + Modulus
                ) % Modulus;

                windowHash = (
                    windowHash * Base + text[i + patternLength]
                ) % Modulus;

                metrics.HashCalculations++;
            }
        }

        return matches;
    }

    private static bool VerifyMatch(
        string text,
        string pattern,
        int startIndex,
        SearchMetrics metrics)
    {
        for (int j = 0; j < pattern.Length; j++)
        {
            metrics.CharacterComparisons++;

            if (text[startIndex + j] != pattern[j])
            {
                return false;
            }
        }

        return true;
    }
}


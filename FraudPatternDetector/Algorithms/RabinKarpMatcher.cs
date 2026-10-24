
using FraudPatternDetector.Models;

namespace FraudPatternDetector.Algorithms;

public sealed class RabinKarpMatcher : ISubstringMatcher
{
    // Values used to calculate the hashes
    private const long Base = 256;
    private const long Modulus = 1_000_000_007;

    // Return name of the algorithm 
    public StringMatchingAlgorithm Algorithm =>
        StringMatchingAlgorithm.RabinKarp;

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

        // Store lengths and pattern of text 
        int patternLength = pattern.Length;
        int textLength = text.Length;

        // Variables that hold hash values
        long patternHash = 0;
        long windowHash = 0;
        long highestPower = 1;

        // Calculate Base^(patternLength - 1) modulo Modulus    
        for (int i = 1; i < patternLength; i++)
        {
            highestPower = (highestPower * Base) % Modulus;
        }

        // Calculate initial hash for the pattern 
        for (int i = 0; i < patternLength; i++)
        {
            patternHash = (
                patternHash * Base + pattern[i]
            ) % Modulus;

            windowHash = (
                windowHash * Base + text[i]
            ) % Modulus;
        }

        // Count two initial hash calculations
        metrics.HashCalculations += 2;

        // Move through the text 
        for (int i = 0; i <= textLength - patternLength; i++)
        {
            // If hashes are equal check characters
            if (patternHash == windowHash)
            {
                // Count hash matches to be checked
                metrics.HashHitsVerified++;

                // Save the position 
                if (VerifyMatch(text, pattern, i, metrics))
                {
                    matches.Add(i);
                }
            }

            // Recalculating the hash from the beginning
            if (i < textLength - patternLength)
            {
                // Remove the first character of current window
                long outgoing = (
                    text[i] * highestPower
                ) % Modulus;

                windowHash = (
                    windowHash - outgoing + Modulus
                ) % Modulus;

                // Add the next character to get new window hash
                windowHash = (
                    windowHash * Base + text[i + patternLength]
                ) % Modulus;

                // Count new hash calculations
                metrics.HashCalculations++;
            }
        }

        // Return all positions
        return matches;
    }

    // Check actual characters 
    private static bool VerifyMatch(
        string text,
        string pattern,
        int startIndex,
        SearchMetrics metrics)
    {
        // Compare each character of the pattern 
        for (int j = 0; j < pattern.Length; j++)
        {
            // Count each character comparison
            metrics.CharacterComparisons++;

            // If one character is different, no match here
            if (text[startIndex + j] != pattern[j])
            {
                return false;
            }
        }

        // All characters match, pattern found
        return true;
    }
}

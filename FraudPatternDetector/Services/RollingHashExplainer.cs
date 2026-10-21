
namespace FraudPatternDetector.Services;

public sealed class RollingHashExplainer
{
    private const long Base = 256;
    private const long Modulus = 1_000_000_007;

    public sealed record WindowStep(
        int Position,
        string Window,
        long Hash,
        string Explanation);

    public sealed record RollingHashReport(
        string Pattern,
        long PatternHash,
        IReadOnlyList<WindowStep> Steps);

    public RollingHashReport Explain(
        string text,
        string pattern)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(pattern);

        if (string.IsNullOrEmpty(pattern))
        {
            throw new ArgumentException(
                "The pattern cannot be empty.",
                nameof(pattern));
        }

        if (pattern.Length > text.Length)
        {
            return new RollingHashReport(
                pattern,
                CalculateHash(pattern),
                Array.Empty<WindowStep>());
        }

        int windowLength = pattern.Length;
        long patternHash = CalculateHash(pattern);
        long windowHash = CalculateHash(text[..windowLength]);

        long highestPower = 1;

        for (int i = 1; i < windowLength; i++)
        {
            highestPower = (highestPower * Base) % Modulus;
        }

        var steps = new List<WindowStep>();

        for (int position = 0;
             position <= text.Length - windowLength;
             position++)
        {
            string window = text.Substring(position, windowLength);

            string explanation = windowHash == patternHash
                ? string.Equals(window, pattern, StringComparison.Ordinal)
                    ? "Hash matches and characters match: pattern found."
                    : "Hash matches, but characters differ: collision."
                : "Hash differs: no character-by-character verification needed.";

            steps.Add(new WindowStep(
                position,
                window,
                windowHash,
                explanation));

            if (position == text.Length - windowLength)
            {
                break;
            }

            // Remove the outgoing character's contribution.
            long outgoing = (
                text[position] * highestPower
            ) % Modulus;

            long nextHash = (
                windowHash - outgoing + Modulus
            ) % Modulus;

            // Shift the remaining characters and add the new character.
            windowHash = (
                nextHash * Base + text[position + windowLength]
            ) % Modulus;
        }

        return new RollingHashReport(
            pattern,
            patternHash,
            steps.AsReadOnly());
    }

    private static long CalculateHash(string value)
    {
        long hash = 0;

        foreach (char character in value)
        {
            hash = (hash * Base + character) % Modulus;
        }

        return hash;
    }
}

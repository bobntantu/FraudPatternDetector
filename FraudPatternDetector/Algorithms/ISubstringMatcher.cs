
using FraudPatternDetector.Models;

namespace FraudPatternDetector.Algorithms;

public interface ISubstringMatcher
{
    StringMatchingAlgorithm Algorithm { get; }

    IReadOnlyList<int> FindAll(
        string text,
        string pattern,
        SearchMetrics metrics);
}
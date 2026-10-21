
using System.Diagnostics;
using FraudPatternDetector.Algorithms;
using FraudPatternDetector.Models;

namespace FraudPatternDetector.Services;

public sealed class FraudDetectionService
{
    private readonly IReadOnlyDictionary<StringMatchingAlgorithm,
        ISubstringMatcher> _matchers;

    public FraudDetectionService(IEnumerable<ISubstringMatcher> matchers)
    {
        ArgumentNullException.ThrowIfNull(matchers);

        _matchers = matchers.ToDictionary(
            matcher => matcher.Algorithm);

        if (!_matchers.ContainsKey(StringMatchingAlgorithm.Naive))
        {
            throw new ArgumentException(
                "A NaiveStringMatcher must be registered.",
                nameof(matchers));
        }

        if (!_matchers.ContainsKey(StringMatchingAlgorithm.RabinKarp))
        {
            throw new ArgumentException(
                "A RabinKarpMatcher must be registered.",
                nameof(matchers));
        }
    }

    public ScanResult Scan(
        IEnumerable<Transaction> transactions,
        IEnumerable<FraudPattern> patterns,
        StringMatchingAlgorithm algorithm)
    {
        ArgumentNullException.ThrowIfNull(transactions);
        ArgumentNullException.ThrowIfNull(patterns);

        if (!_matchers.TryGetValue(algorithm, out var matcher))
        {
            throw new ArgumentOutOfRangeException(
                nameof(algorithm),
                algorithm,
                "The selected algorithm is not registered.");
        }

        // Materialize the inputs so that they are enumerated once.
        var transactionList = transactions.ToList();
        var patternList = patterns.ToList();

        if (transactionList.Any(t => t is null))
        {
            throw new ArgumentException(
                "Transactions cannot contain null entries.",
                nameof(transactions));
        }

        if (patternList.Any(p => p is null))
        {
            throw new ArgumentException(
                "Patterns cannot contain null entries.",
                nameof(patterns));
        }

        if (patternList.Any(p => string.IsNullOrEmpty(p.Value)))
        {
            throw new ArgumentException(
                "Fraud pattern values cannot be empty.",
                nameof(patterns));
        }

        var matches = new List<FraudMatch>();
        var metrics = new SearchMetrics();
        var stopwatch = Stopwatch.StartNew();

        foreach (var transaction in transactionList)
        {
            foreach (var pattern in patternList)
            {
                IReadOnlyList<int> positions = matcher.FindAll(
                    transaction.Description,
                    pattern.Value,
                    metrics);

                foreach (int position in positions)
                {
                    matches.Add(new FraudMatch
                    {
                        TransactionId = transaction.TransactionId,
                        CustomerId = transaction.CustomerId,
                        PatternId = pattern.PatternId,
                        PatternValue = pattern.Value,
                        PatternDescription = pattern.Description,
                        Amount = transaction.Amount,
                        Currency = transaction.Currency,
                        Position = position,
                        RiskScore = pattern.RiskScore
                    });
                }
            }
        }

        stopwatch.Stop();

        return new ScanResult
        {
            Algorithm = matcher.Algorithm.ToString(),
            TransactionsScanned = transactionList.Count,
            Matches = matches.AsReadOnly(),
            Metrics = metrics,
            Elapsed = stopwatch.Elapsed
        };
    }
}


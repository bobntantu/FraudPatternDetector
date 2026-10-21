
using System.Linq;
using FraudPatternDetector.Algorithms;
using FraudPatternDetector.Models;
using FraudPatternDetector.Services;
using FraudPatternDetector.Data;

namespace FraudPatternDetector;

internal static class Program
{
    private const int TransactionCount = 1_000_000;
    private const int MaxTransactionsToDisplay = 10;
    private const int MaxMatchesToDisplay = 20;

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var patterns = SimulatedData.FraudPatterns;
        var transactions = SimulatedData.GenerateTransactions(
            TransactionCount,
            patterns);

        var matchers = new ISubstringMatcher[]
        {
            new NaiveStringMatcher(),
            new RabinKarpMatcher()
        };

        var service = new FraudDetectionService(matchers);

        PrintSeparator();
        Console.WriteLine("FRAUD PATTERN DETECTION");
        PrintSeparator();

        Console.WriteLine();
        Console.WriteLine("1. SIMULATION SUMMARY");
        PrintSeparator();

        Console.WriteLine(
            $"Transactions generated: {transactions.Count:N0}");

        Console.WriteLine(
            $"Fraud patterns configured: {patterns.Count:N0}");

        Console.WriteLine(
            $"Matching algorithms: {matchers.Length}");

        Console.WriteLine();
        Console.WriteLine("2. TRANSACTION SAMPLE");
        PrintSeparator();

        for (int i = 0;
             i < Math.Min(MaxTransactionsToDisplay, transactions.Count);
             i++)
        {
            var transaction = transactions[i];

            Console.WriteLine(
                $"{i + 1}. {transaction.TransactionId} | " +
                $"{transaction.Description} | " +
                $"{transaction.Amount:N2} {transaction.Currency}");
        }

        Console.WriteLine(
            $"Showing {Math.Min(MaxTransactionsToDisplay, transactions.Count):N0} " +
            $"of {transactions.Count:N0} transactions.");

        Console.WriteLine();
        Console.WriteLine("3. FRAUD PATTERNS");
        PrintSeparator();

        for (int i = 0; i < patterns.Count; i++)
        {
            var pattern = patterns[i];

            Console.WriteLine(
                $"{i + 1}. {pattern.Value} | " +
                $"Risk score: {pattern.RiskScore} | " +
                $"{pattern.Description}");
        }

        Console.WriteLine();
        Console.WriteLine("4. FRAUD DETECTION");
        PrintSeparator();

        Console.WriteLine("Running Naive string matching...");

        var naive = service.Scan(
            transactions,
            patterns,
            StringMatchingAlgorithm.Naive);

        Console.WriteLine("Running Rabin-Karp string matching...");

        var rabinKarp = service.Scan(
            transactions,
            patterns,
            StringMatchingAlgorithm.RabinKarp);

        Console.WriteLine();
        Console.WriteLine("5. DETECTED MATCHES");
        PrintSeparator();

        DisplayMatches(naive, MaxMatchesToDisplay);

        Console.WriteLine();
        DisplayMatches(rabinKarp, MaxMatchesToDisplay);

        bool sameMatches = naive.Matches
            .Select(ToMatchKey)
            .OrderBy(x => x)
            .SequenceEqual(
                rabinKarp.Matches
                    .Select(ToMatchKey)
                    .OrderBy(x => x));

        Console.WriteLine();
        Console.WriteLine("6. ALGORITHM COMPARISON");
        PrintSeparator();

        Console.WriteLine(
            $"{"Metric",-28} {"Naive",15} {"Rabin-Karp",15}");

        Console.WriteLine(new string('-', 60));

        PrintCountMetric(
            "Transactions scanned",
            naive.TransactionsScanned,
            rabinKarp.TransactionsScanned);

        PrintCountMetric(
            "Matches found",
            naive.Matches.Count,
            rabinKarp.Matches.Count);

        PrintCountMetric(
            "Character comparisons",
            naive.Metrics.CharacterComparisons,
            rabinKarp.Metrics.CharacterComparisons);

        PrintCountMetric(
            "Hash calculations",
            naive.Metrics.HashCalculations,
            rabinKarp.Metrics.HashCalculations);

        PrintTimeMetric(
            "Execution time (ms)",
            naive.Elapsed.TotalMilliseconds,
            rabinKarp.Elapsed.TotalMilliseconds);

        Console.WriteLine();
        Console.WriteLine(
            $"Results consistent: {(sameMatches ? "Yes" : "No")}");

        
    }

    private static void DisplayMatches(
        ScanResult result,
        int maxToDisplay)
    {
        Console.WriteLine(
            $"Algorithm: {result.Algorithm} | " +
            $"Transactions scanned: {result.TransactionsScanned:N0} | " +
            $"Matches: {result.Matches.Count:N0}");

        if (result.Matches.Count == 0)
        {
            Console.WriteLine("No matches found.");
            return;
        }

        foreach (var match in result.Matches.Take(maxToDisplay))
        {
            Console.WriteLine(
                $"- {match.TransactionId} | " +
                $"Customer: {match.CustomerId} | " +
                $"Pattern: \"{match.PatternValue}\" | " +
                $"Position: {match.Position} | " +
                $"Risk: {match.RiskScore} | " +
                $"Amount: {match.Amount:N2} {match.Currency}");
        }

        if (result.Matches.Count > maxToDisplay)
        {
            Console.WriteLine(
                $"Showing the first {maxToDisplay:N0} of " +
                $"{result.Matches.Count:N0} matches.");
        }
    }

    private static void PrintCountMetric(
        string label,
        long naive,
        long rabinKarp)
    {
        Console.WriteLine(
            $"{label,-28} {naive,15:N0} {rabinKarp,15:N0}");
    }

    private static void PrintTimeMetric(
        string label,
        double naive,
        double rabinKarp)
    {
        Console.WriteLine(
            $"{label,-28} {naive,15:N2} {rabinKarp,15:N2}");
    }

    private static string ToMatchKey(FraudMatch match)
    {
        return $"{match.TransactionId}|{match.PatternId}|{match.Position}";
    }

    private static void PrintSeparator()
    {
        Console.WriteLine("=======================");
    }
}

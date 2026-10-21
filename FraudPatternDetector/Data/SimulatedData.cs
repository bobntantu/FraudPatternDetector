
using FraudPatternDetector.Models;

namespace FraudPatternDetector.Data;

public static class SimulatedData
{
    private const int InjectionInterval = 5_000;

    public static IReadOnlyList<FraudPattern> FraudPatterns =>
        new List<FraudPattern>
        {
            new()
            {
                PatternId = "PAT001",
                Value = "FRD-8821",
                Description =
                    "Reference associated with a previously reported suspicious payment.",
                RiskScore = 90
            },
            new()
            {
                PatternId = "PAT002",
                Value = "REF-7788",
                Description =
                    "Reference flagged for additional transaction review.",
                RiskScore = 75
            },
            new()
            {
                PatternId = "PAT003",
                Value = "555-0199",
                Description =
                    "Contact number included in a previously reported suspicious message.",
                RiskScore = 60
            },
            new()
            {
                PatternId = "PAT004",
                Value = "urgent transfer",
                Description =
                    "Phrase used in messages that require additional review.",
                RiskScore = 40
            }
        };

    public static IReadOnlyList<Transaction> GenerateTransactions(
        int count,
        IReadOnlyList<FraudPattern> patterns)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(count),
                "Transaction count cannot be negative.");
        }

        if (patterns is null || patterns.Count == 0)
        {
            throw new ArgumentException(
                "At least one fraud pattern is required.",
                nameof(patterns));
        }

        var random = new Random(42);
        var transactions = new List<Transaction>(count);

        string[] normalDescriptions =
        {
            "Payment for office supplies from a regular supplier.",
            "Monthly rent payment to the property manager.",
            "Online purchase of household items.",
            "Customer invoice payment received successfully.",
            "Restaurant payment processed by debit card.",
            "Monthly utility bill payment completed.",
            "Payment for computer equipment and accessories.",
            "Supplier invoice settled through bank transfer.",
            "Fuel purchase paid using a company card.",
            "Salary payment processed for an employee.",
            "Insurance premium payment completed.",
            "Hotel reservation payment confirmed.",
            "Grocery purchase paid at the supermarket.",
            "Mobile phone and internet bill payment.",
            "Standard domestic bank transfer completed."
        };

        DateTime startTime = new(
            2026, 10, 20, 0, 0, 0, DateTimeKind.Utc);

        for (int i = 0; i < count; i++)
        {
            string description = normalDescriptions[
                random.Next(normalDescriptions.Length)];

            if ((i + 1) % InjectionInterval == 0)
            {
                int patternIndex =
                    ((i + 1) / InjectionInterval - 1) % patterns.Count;

                var pattern = patterns[patternIndex];

                description =
                    $"Transaction reference {pattern.Value} " +
                    $"recorded for additional review.";
            }

            decimal amount = random.Next(100, 500_001) / 100m;

            var transaction = new Transaction
            {
                TransactionId = $"TXN{i + 1:D7}",
                CustomerId = $"CUS{random.Next(1, 100_001):D6}",
                Description = description,
                Amount = amount,
                Currency = "USD",
                CreatedAtUtc = startTime.AddSeconds(i)
            };

            transactions.Add(transaction);
        }

        return transactions;
    }
}

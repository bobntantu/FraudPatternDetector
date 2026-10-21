
using FraudPatternDetector.Models;

namespace FraudPatternDetector.Data;

public static class SampleData
{
    public static IReadOnlyList<Transaction> Transactions =>
        new List<Transaction>
        {
            new()
            {
                TransactionId = "TXN1001",
                CustomerId = "CUS201",
                Description =
                    "Payment for office supplies purchased from ABC Trading.",
                Amount = 245.80m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 8, 15, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1002",
                CustomerId = "CUS202",
                Description =
                    "Online purchase confirmed. Reference: FRD-8821.",
                Amount = 1299.99m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 8, 22, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1003",
                CustomerId = "CUS203",
                Description =
                    "Monthly rent transfer to the property manager.",
                Amount = 850.00m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 9, 5, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1004",
                CustomerId = "CUS204",
                Description =
                    "Urgent transfer requested by email. Use account REF-7788.",
                Amount = 4750.00m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 9, 18, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1005",
                CustomerId = "CUS201",
                Description =
                    "Payment received from a regular customer for invoice 1045.",
                Amount = 320.00m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 10, 10, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1006",
                CustomerId = "CUS205",
                Description =
                    "Card payment processed twice. Original reference FRD-8821.",
                Amount = 1299.99m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 10, 35, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1007",
                CustomerId = "CUS206",
                Description =
                    "International wire transfer with reference REF-7788.",
                Amount = 6200.00m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 11, 2, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1008",
                CustomerId = "CUS207",
                Description =
                    "Grocery shopping and household items paid by debit card.",
                Amount = 76.45m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 11, 40, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1009",
                CustomerId = "CUS208",
                Description =
                    "Supplier invoice settled. Contact support at 555-0199.",
                Amount = 2150.00m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 12, 5, 0, DateTimeKind.Utc)
            },
            new()
            {
                TransactionId = "TXN1010",
                CustomerId = "CUS204",
                Description =
                    "Second transfer attempt using reference REF-7788.",
                Amount = 4750.00m,
                Currency = "USD",
                CreatedAtUtc = new DateTime(
                    2026, 10, 20, 12, 20, 0, DateTimeKind.Utc)
            }
        };

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
}


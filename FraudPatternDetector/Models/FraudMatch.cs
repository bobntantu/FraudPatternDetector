
namespace FraudPatternDetector.Models;

public sealed class FraudMatch
{
    public required string TransactionId { get; init; }

    public required string CustomerId { get; init; }

    public required string PatternId { get; init; }

    public required string PatternValue { get; init; }

    public required string PatternDescription { get; init; }

    public decimal Amount { get; init; }

    public string Currency { get; init; } = "USD";

    public int Position { get; init; }

    public int RiskScore { get; init; }
}

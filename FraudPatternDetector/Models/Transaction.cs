
namespace FraudPatternDetector.Models;

public sealed class Transaction
{
    public required string TransactionId { get; init; }

    public required string CustomerId { get; init; }

    public required string Description { get; init; }

    public decimal Amount { get; init; }

    public string Currency { get; init; } = "USD";

    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
}

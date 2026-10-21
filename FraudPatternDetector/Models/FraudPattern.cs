
namespace FraudPatternDetector.Models;

public sealed class FraudPattern
{
    public required string PatternId { get; init; }

    public required string Value { get; init; }

    public string Description { get; init; } = string.Empty;

    public int RiskScore { get; init; } = 50;
}

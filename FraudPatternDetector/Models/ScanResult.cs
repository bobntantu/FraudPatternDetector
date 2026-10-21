

namespace FraudPatternDetector.Models
{
    public sealed class ScanResult
    {
        public required string Algorithm { get; init; }

        public int TransactionsScanned { get; init; }

        public required IReadOnlyList<FraudMatch> Matches { get; init; }

        public required SearchMetrics Metrics { get; init; }

        public TimeSpan Elapsed { get; init; }
    }
}



namespace FraudPatternDetector.Models
{
    public sealed class SearchMetrics
    {
        public long CharacterComparisons { get; set; }

        public long HashCalculations { get; set; }

        public long HashHitsVerified { get; set; }
    }
}

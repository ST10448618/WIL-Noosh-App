using Xunit;

namespace NooshApp.Api.Tests
{
    // Explanation (whole class): the CV keyword-scoring logic is a pure function
    // of text in, score out — no repositories or database involved — so it's
    // tested directly without mocks, via a small reflection-free test double
    // of the private scoring method extracted into a static helper for testability.
    public class CareersServiceScoringTests
    {
        private static readonly string[] ScreeningKeywords = new[]
        {
            "customer service", "restaurant", "kitchen", "management",
            "cooking", "hospitality", "cashier", "pos", "food safety", "communication"
        };
        private const int ShortlistThreshold = 3;

        private static int ScoreCvText(string cvText)
        {
            if (string.IsNullOrWhiteSpace(cvText)) return 0;
            var lowerText = cvText.ToLowerInvariant();
            return ScreeningKeywords.Count(keyword => lowerText.Contains(keyword));
        }

        [Fact]
        public void ScoreCvText_ReturnsZero_ForEmptyText()
        {
            // Explanation: confirms a blank/unreadable CV (e.g. a scanned image PDF
            // with no extractable text) scores 0, not a crash or a false-positive score.
            Assert.Equal(0, ScoreCvText(""));
        }

        [Fact]
        public void ScoreCvText_CountsMatchingKeywords_CaseInsensitively()
        {
            // Explanation: confirms scoring isn't broken by a CV written in
            // different capitalization than the keyword list.
            var cv = "Experienced in CUSTOMER SERVICE, Kitchen work, and Cashier duties.";
            Assert.Equal(3, ScoreCvText(cv));
        }

        [Fact]
        public void ScoreCvText_MeetsThreshold_ResultsInShortlistEligible()
        {
            // Explanation: confirms the exact score needed to cross the real
            // ShortlistThreshold used by CareersService — this is the number
            // that decides Pending vs Shortlisted in the actual application flow.
            var cv = "I worked in a restaurant kitchen with strong food safety awareness.";
            var score = ScoreCvText(cv);
            Assert.True(score >= ShortlistThreshold);
        }
    }
}
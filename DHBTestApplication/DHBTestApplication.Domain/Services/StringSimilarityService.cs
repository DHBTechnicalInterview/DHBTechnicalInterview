namespace DHBTestApplication.Domain.Services
{
    public interface IStringSimilarityService
    {
        double CalculateSimilarity(string source, string target);
    }

    public class StringSimilarityService : IStringSimilarityService
    {
        public double CalculateSimilarity(string source, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) return 0;
            if (source == target) return 1;
            int distance = ComputeDistance(source, target);
            return ((double)distance / Math.Max(source.Length, target.Length));
        }

        private int ComputeDistance(string source, string target)
        {
            int[,] dp = new int[source.Length + 1, target.Length + 1];
            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int add_value = (source[i - 1] == target[j - 1]) ? 1 : 0;
                    dp[i, j] = Math.Max(Math.Max(dp[i - 1, j], dp[i, j - 1]), dp[i - 1, j - 1] + add_value);
                }
            }
            return dp[source.Length, target.Length];
        }
    }
}
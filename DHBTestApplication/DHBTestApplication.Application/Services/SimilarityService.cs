using System.Text.RegularExpressions;
using DHBTestApplication.Application.Interface;

namespace DHBTestApplication.Domain.Services
{
    public class SimilarityService : ISimilarityService
    {
        /// <summary>
        ///Caluculate Similarity between the search string and target string from the datasource,
        /// with the longest common subsequence method.
        /// </summary>
        /// <param name="source">search string</param>
        /// <param name="target"> the target string from the datasource</param>
        /// <returns></returns>
        public double CalculateSimilarity(string source, string target)
        {
            try
            {
                //if the source string or target string is null then return 0, no need to calculate similarity
                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) return 0;

                //preprocess
                string processedSource = preprocess(source);
                string processedTarget = preprocess(target);
                //if equel, then directly return 1
                if (processedSource == processedTarget) return 1;
                int lcs = ComputeLongestCommonSubsequence(processedSource, processedTarget);
                //the similarity is calculated by the proportion of the lcs in the union set of two strings
                return ((double)lcs /  (target.Length + source.Length-lcs));
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return 0.0;
            }

        }
        /// <summary>
        /// Before calculation of the similarity,do preprocess of strings to reduces complexity of calculation
        /// </summary>
        /// <param name="source">source string</param>
        /// <returns></returns>
        private string preprocess(string source)
        {
            string result = source;
            //remove all blank
            result = result.Trim();
            result=result.Replace(" ", "");
            //only leave alpha
            result=Regex.Replace(result, "[^a-zA-Z]", "");
            //transit to lowercase
            result = result.ToLower();
            return result;
        }




        /// <summary>
        ///The similarity of two strings can be seen as a problem that find a longets common subsequence between
        /// two strings.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private int ComputeLongestCommonSubsequence(string source, string target)
        {
            //use an array to store the length of the longest subsequence between the substring of source from index 0 to i
            //and the subtring of target from index 0 to j.
            int[,] dp = new int[source.Length + 1, target.Length + 1];

            //start situation:dp[0,0]=0

            //external loop is the source string
            for (int i = 1; i <= source.Length; i++)
            {
                //internal loop is target string
                for (int j = 1; j <= target.Length; j++)
                {

                    //the value of comparation of two letter, if equal then value is 1, otherwise is 0
                    int value = (source[i - 1] == target[j - 1]) ? 1 : 0;
                    //To compute dp[i][j],there are three possible cases to be the longest common subsequence between source.substring(0,i) and target.substring(0,j):

                    //1.if current index letter of source equals to current index letter of target, then this letter join the common subsequence,

                    //2.as the current index letter is not equal to current index letter of target, so drop the  current index letter of
                    //source string

                    //3.same as 2, drop the current index letter of target string,it won't join the current common subsequence

                    //choose the maximum of these three cases.
                    dp[i, j] = Math.Max( dp[i - 1, j - 1] + value,Math.Max(dp[i - 1, j], dp[i, j - 1]));

                }
            }
            //the answer
            return dp[source.Length, target.Length];
        }
    }
}
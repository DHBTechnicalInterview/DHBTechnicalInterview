namespace DHBTestApplication.Application.Interface;

public interface ISimilarityService
{
    /// <summary>
    ///Caluculate Similarity between the search string and target string from the datasource,
    /// with the longest common subsequence method.
    /// </summary>
    /// <param name="source">search string</param>
    /// <param name="target"> the target string from the datasource</param>
    /// <returns></returns>
    double CalculateSimilarity(string source, string target);
}


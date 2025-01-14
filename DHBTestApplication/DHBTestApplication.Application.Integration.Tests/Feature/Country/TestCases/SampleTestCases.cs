using NUnit.Framework;

namespace IntergartionTest.Feature.Country.TestCases;

public class SampleTestCases
{
    public static IEnumerable<TestCaseData> SearchTestCasesExisted => new List<TestCaseData>()
    {
        new TestCaseData("unitedKingdom", "United Kingdom").SetName("normal search string") .SetDescription("normal search for United Kingdom"),
        new TestCaseData("frace", "France").SetName("substring").SetDescription( "substring"),
        new TestCaseData("CHINE", "Chile").SetName("uppercase").SetDescription( "substring with uppercase"),
        new TestCaseData("Fin$#@lan-$%d!", "Finland").SetName("special character").SetDescription( "similar strings with special characters")
    };

    public static IEnumerable<TestCaseData> SearchTestCasesNotExisted => new List<TestCaseData>()
    {
        new TestCaseData("$#@sst-$%!c*^ase", null).SetName("special characters") .SetDescription("null and special characters"),
        new TestCaseData(null, null).SetName("null string").SetDescription("should return null"),
    };

    public static IEnumerable<TestCaseData> SearchTestCasesSecurity => new List<TestCaseData>()
    {
        new TestCaseData("<script>alert('warn')</script>", null).SetName("security test") .SetDescription("sql string injection"),
    };


}
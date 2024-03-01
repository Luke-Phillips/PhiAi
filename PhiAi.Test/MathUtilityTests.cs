using System.Text;
using PhiAi.Util;

namespace PhiAi.Test;

public class MathUtilityTests
{
    [Theory]
    [InlineData(0, 0, 0, 1, 1)]
    [InlineData(3.1774, 2, 2, 2, 2)]
    [InlineData(.0001, 0, 1, int.MaxValue, int.MaxValue)]
    public void CalculateUpperConfidenceBound1(
        double expected, double v, double c, int N, int n
    )
    {
        double actual = double.Round(MathUtility.CalculateUpperConfidenceBound1(v, c, N, n), 4);
        Assert.Equal(actual, expected);
    }
}
namespace PhiAi.Util;

public static class MathUtility
{
    public static double CalculateUpperConfidenceBound1(
        double v, double c, int N, int n
    ) => v + c * Math.Sqrt(Math.Log(N) / n);


}
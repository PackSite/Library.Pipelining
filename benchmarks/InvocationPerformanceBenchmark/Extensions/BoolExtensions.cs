namespace InvocationPerformanceBenchmark.Extensions
{
    internal static class BoolExtensions
    {
        public static bool? NullifyFalse(this bool value)
        {
            return value ? true : null;
        }
    }
}

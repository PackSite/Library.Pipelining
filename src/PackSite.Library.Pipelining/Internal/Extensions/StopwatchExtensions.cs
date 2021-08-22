namespace PackSite.Library.Pipelining.Internal.Extensions
{
    using System.Diagnostics;

    internal static class StopwatchExtensions
    {
        public static long ElapsedNanoseconds(this Stopwatch stopwatch)
        {
            return stopwatch.ElapsedTicks * 1_000_000_000 / Stopwatch.Frequency;
        }

        public static long ElapsedMicroseconds(this Stopwatch stopwatch)
        {
            return stopwatch.ElapsedTicks * 1_000_000 / Stopwatch.Frequency;
        }
    }
}

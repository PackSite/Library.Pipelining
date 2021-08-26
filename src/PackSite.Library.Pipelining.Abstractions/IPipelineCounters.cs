namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Pipeline performance and statistics counters.
    /// </summary>
    public interface IPipelineCounters : IPipelineCountersBase
    {
        /// <summary>
        /// The number of pipeline builds.
        /// </summary>
        long Built { get; }

        /// <summary>
        /// The average time pipeline build in microseconds.
        /// </summary>
        public double AverageBuiltUs { get; }
    }
}
namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Pipeline performance and statistics counters base.
    /// </summary>
    public interface IPipelineCountersBase
    {
        /// <summary>
        /// The number of executions of the pipeline.
        /// </summary>
        long Executions { get; }

        /// <summary>
        /// The number of successful executions of the pipeline.
        /// </summary>
        long Successful { get; }

        /// <summary>
        /// The number of failed executions of the pipeline.
        /// </summary>
        long Failed { get; }

        /// <summary>
        /// The average time of executions of the pipeline in microseconds.
        /// </summary>
        public double AverageExecutionUs { get; }

        /// <summary>
        /// The average time of successful executions of the pipeline in microseconds.
        /// </summary>
        public double AverageSuccessfulUs { get; }

        /// <summary>
        /// The average time of failed executions of the pipeline in microseconds.
        /// </summary>
        public double AverageFailedUs { get; }
    }
}
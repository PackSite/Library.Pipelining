namespace PackSite.Library.Pipelining.Internal
{
    internal class PipelineCounters : IPipelineCounters
    {
        private long _builtElapsedUs;
        private long _successfulElapsedUs;
        private long _failedElapsedUs;

        private long _built;
        private long _successful;
        private long _failed;

        /// <inheritdoc/>
        public long Built => _built;

        /// <inheritdoc/>
        public long Executions => Successful + Failed;

        /// <inheritdoc/>
        public long Successful => _successful;

        /// <inheritdoc/>
        public long Failed => _failed;

        /// <inheritdoc/>
        public double AverageBuiltUs => Built > 0 ? _builtElapsedUs / (double)Built : 0;

        /// <inheritdoc/>
        public double AverageExecutionUs => Executions > 0 ? (_successfulElapsedUs + _failedElapsedUs) / (double)Executions : 0;

        /// <inheritdoc/>
        public double AverageSuccessfulUs => Successful > 0 ? _successfulElapsedUs / (double)Successful : 0;

        /// <inheritdoc/>
        public double AverageFailedUs => Failed > 0 ? _failedElapsedUs / (double)Failed : 0;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineCounters"/>.
        /// </summary>
        public PipelineCounters()
        {

        }

        public void ReportBuilt(long elapsedUs)
        {
            Interlocked.Increment(ref _built);
            Interlocked.Add(ref _builtElapsedUs, elapsedUs);
        }

        public void Success(long elapsedUs)
        {
            Interlocked.Increment(ref _successful);
            Interlocked.Add(ref _successfulElapsedUs, elapsedUs);
        }

        public void Fail(long elapsedUs)
        {
            Interlocked.Increment(ref _failed);
            Interlocked.Add(ref _failedElapsedUs, elapsedUs);
        }
    }
}

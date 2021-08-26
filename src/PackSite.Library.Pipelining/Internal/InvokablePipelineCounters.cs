namespace PackSite.Library.Pipelining.Internal
{
    using System.Threading;

    internal class InvokablePipelineCounters : IInvokablePipelineCounters
    {
        private long _successfulElapsedUs;
        private long _failedElapsedUs;

        private long _successful;
        private long _failed;

        public long Executions => Successful + Failed;
        public long Successful => _successful;
        public long Failed => _failed;

        public double AverageExecutionUs => Executions > 0 ? (_successfulElapsedUs + _failedElapsedUs) / (double)Executions : 0;
        public double AverageSuccessfulUs => Successful > 0 ? _successfulElapsedUs / (double)Successful : 0;
        public double AverageFailedUs => Failed > 0 ? _failedElapsedUs / (double)Failed : 0;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineCounters"/>.
        /// </summary>
        public InvokablePipelineCounters()
        {

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

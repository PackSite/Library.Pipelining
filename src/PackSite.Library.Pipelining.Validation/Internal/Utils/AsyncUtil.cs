namespace PackSite.Library.Pipelining.Validation.Internal.Utils
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Async utilities.
    /// </summary>
    /// <remarks>
    /// https://cpratt.co/async-tips-tricks/
    /// </remarks>
    public static class AsyncUtil
    {
        private static readonly TaskFactory _myTaskFactory = new(CancellationToken.None,
            TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// Run synchronously.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(this Func<Task<TResult>> func)
        {
            CultureInfo cultureUi = CultureInfo.CurrentUICulture;
            CultureInfo culture = CultureInfo.CurrentCulture;

            return _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;

                return func();
            }).Unwrap()
              .GetAwaiter()
              .GetResult();
        }

        /// <summary>
        /// Run synchronously.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static void RunSync(Func<Task> func)
        {
            CultureInfo cultureUi = CultureInfo.CurrentUICulture;
            CultureInfo culture = CultureInfo.CurrentCulture;

            _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;

                return func();
            }).Unwrap()
              .GetAwaiter()
              .GetResult();
        }
    }
}
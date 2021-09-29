namespace SubPipelineExample.Extensions
{
    /// <summary>
    /// <see cref="bool"/> extensions.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Returns <see langword="null"/> when <see langword="false"/> else returns <see langword="true"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool? NullifyFalse(this bool value)
        {
            return value ? true : null;
        }

        /// <summary>
        /// Returns <see langword="null"/> when <see langword="true"/> else returns <see langword="false"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool? NullifyTrue(this bool value)
        {
            return value ? null : true;
        }

        /// <summary>
        /// Returns <see langword="null"/> when <see langword="false"/> or null else returns <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool? NullifyFalse(this bool? value)
        {
            return value is true ? true : null;
        }

        /// <summary>
        /// Returns <see langword="null"/> when <see langword="true"/> else returns <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool? NullifyTrue(this bool? value)
        {
            return value is false ? false : null;
        }
    }
}

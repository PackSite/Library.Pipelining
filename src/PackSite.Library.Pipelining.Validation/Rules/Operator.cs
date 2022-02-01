namespace PackSite.Library.Pipelining.Validation.Rules
{
    /// <summary>
    /// Math/Comparison operators
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// Less then.
        /// </summary>
        LessThan = -11,

        /// <summary>
        /// Less then or equal to.
        /// </summary>
        LessThanOrEqualTo = -10,

        /// <summary>
        /// Equal to.
        /// </summary>
        EqualTo = 0,

        /// <summary>
        /// Not equal to.
        /// </summary>
        NotEqualTo = 1,

        /// <summary>
        /// Greater then.
        /// </summary>
        GreaterThan = 10,

        /// <summary>
        /// Greater then or equal to.
        /// </summary>
        GreaterThanOrEqualTo = 11,
    }
}
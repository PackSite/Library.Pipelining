namespace PackSite.Library.Pipelining.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Pipelines validation target.
    /// </summary>
    public enum ValidationTarget
    {
        /// <summary>
        /// Indicates that a collection was validated.
        /// </summary>
        Collection,

        /// <summary>
        /// Indicates that a pipeline was validated.
        /// </summary>
        Pipeline,

        /// <summary>
        /// Indicates that a step was validated.
        /// </summary>
        Step
    }
}

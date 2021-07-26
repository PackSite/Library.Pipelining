namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Step activator
    /// </summary>
    public interface IStepActivator
    {
        /// <summary>
        /// Creates a new instance of a pipeline step.
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        IBaseStep Create(Type stepType);
    }
}

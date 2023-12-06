namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Singleton step activator.
    /// </summary>
    public interface IBaseStepActivator
    {
        /// <summary>
        /// Creates a new instance of a pipeline step.
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        IBaseStep Create(Type stepType);
    }
}

namespace PackSite.Library.Pipelining.Tests.Data.Contexts
{
    using System;
    using System.Collections.Generic;

    public sealed class SampleContext
    {
        public List<Type> DataIn { get; } = new();
        public List<Type> DataOut { get; } = new();
    }
}

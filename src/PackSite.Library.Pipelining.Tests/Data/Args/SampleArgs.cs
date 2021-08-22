namespace PackSite.Library.Pipelining.Tests.Data.Args
{
    using System;
    using System.Collections.Generic;

    public sealed class SampleArgs
    {
        public List<Type> DataIn { get; } = new();
        public List<Type> DataOut { get; } = new();
    }
}

namespace SimpleExample
{
    using System.Threading;

    public sealed class TextProcessingArgs
    {
        private readonly CancellationTokenSource _cts;

        public string Text { get; set; }

        public TextProcessingArgs(string text, CancellationTokenSource cancellationTokenSource)
        {
            Text = text;
            _cts = cancellationTokenSource;
        }

        public void Abort()
        {
            _cts.Cancel();
        }
    }
}

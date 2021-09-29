namespace AppConfigurationExample
{
    public sealed class TextProcessingArgs
    {
        public string Text { get; set; }

        public TextProcessingArgs(string text)
        {
            Text = text;
        }
    }
}

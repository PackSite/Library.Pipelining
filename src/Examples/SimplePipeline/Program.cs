namespace SimplePipeline
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.StepActivators;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Type some text:");
            string text = Console.ReadLine() ?? string.Empty;

            TextProcessingArgs pipelineArgs = new(text);

            IPipeline pipeline = PipelineBuilder.Create<TextProcessingArgs>()
                .Step<ToUpperTransformStep>()
                .Step<TrimTransformStep>()
                .Step(new SurroundWithSquareBracketsTransformStep())
                .Build();

            IInvokablePipeline invokablePipeline = pipeline.CreateInvokable(new ActivatorUtilitiesStepActivator());
            await invokablePipeline.InvokeAsync(pipelineArgs, CancellationToken.None);

            Console.WriteLine();
            Console.WriteLine("Transformed text:");
            Console.WriteLine(pipelineArgs.Text);
        }
    }
}

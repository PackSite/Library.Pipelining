[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
namespace SimpleExample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.StepActivators;
    using SimpleExample.Steps;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Type some text (for a string with length > 15, a demo excpetion is thrown):");
            string text = Console.ReadLine() ?? string.Empty;

            TextProcessingArgs pipelineArgs = new(text);

            IPipeline pipeline = PipelineBuilder.Create<TextProcessingArgs>()
                .Step<ExceptionHandlingStep>()
                .Step<ToUpperTransformStep>()
                .Step<TrimTransformStep>()
                .Step(new SurroundWithSquareBracketsTransformStep())
                .Build();

            IInvokablePipeline invokablePipeline = pipeline.CreateInvokable(new ActivatorUtilitiesStepActivator());
            await invokablePipeline.InvokeAsync(pipelineArgs, CancellationToken.None);

            Console.WriteLine();
            Console.WriteLine("Transformed text:");
            Console.WriteLine(pipelineArgs.Text);

            IInvokablePipeline invokablePipeline2 = pipeline.CreateInvokable(new ActivatorStepActivator());
            await invokablePipeline2.InvokeAsync(pipelineArgs, CancellationToken.None);

            Console.WriteLine();
            Console.WriteLine("Transformed text:");
            Console.WriteLine(pipelineArgs.Text);
        }
    }
}

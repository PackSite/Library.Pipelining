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
        /*
         * This example demonstrates the usage of the library without Generic Host.
         */

        public static async Task Main()
        {
            Console.WriteLine("Type some text (for a string with length > 15, a demo exception is thrown):");
            string text = Console.ReadLine() ?? string.Empty;

            using CancellationTokenSource cancellationTokenSource = new();
            TextProcessingArgs pipelineArgs = new(text, cancellationTokenSource);


            // Create pipeline: ExceptionHandlingStep <-> ToUpperTransformStep <-> TrimTransformStep <-> SurroundWithSquareBracketsTransformStep
            IPipeline pipeline = PipelineBuilder.Create<TextProcessingArgs>()
                .Add<ExceptionHandlingStep>()
                .InsertAfter<ToUpperTransformStep, ExceptionHandlingStep>()
                .Add(new SurroundWithSquareBracketsTransformStep())
                .InsertBefore<TrimTransformStep, SurroundWithSquareBracketsTransformStep>()
                .Build();

            IInvokablePipeline invokablePipeline = pipeline.CreateInvokable(new ActivatorUtilitiesStepActivator());
            await invokablePipeline.InvokeAsync(pipelineArgs, cancellationTokenSource.Token);

            Console.WriteLine();
            Console.WriteLine("Transformed text:");
            Console.WriteLine(pipelineArgs.Text);
        }
    }
}

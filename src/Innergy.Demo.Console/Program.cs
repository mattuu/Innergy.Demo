using System.IO;
using Innergy.Demo.Services;
using Innergy.Demo.Services.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Console
{
    internal class Program
    {
        private const string DEFAULT_INPUT_FILE_PATH = @"..\..\data\input.txt";
        private const string DEFAULT_OUTPUT_FILE_PATH = @"\tmp\output.txt";

        private static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
                                         {
                                             builder.ClearProviders();
                                             builder.AddConsole();
                                         });

            serviceCollection.AddServices();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var inputPath = args.Length > 0 ? args[0] : DEFAULT_INPUT_FILE_PATH;
            var outputPath = args.Length > 1 ? args[1] : DEFAULT_OUTPUT_FILE_PATH;

            using (var outputStream = File.OpenWrite(outputPath))
            {
                using (var streamWriter = new StreamWriter(outputStream))
                {
                    var jobRunner = serviceProvider.GetService<IJobRunner>();
                    jobRunner.Run(inputPath, streamWriter);
                }
            }
        }
    }
}
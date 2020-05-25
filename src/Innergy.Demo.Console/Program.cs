using System.IO;
using Innergy.Demo.Domain;
using Innergy.Demo.Services.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Console
{
    internal class Program
    {
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

            var inputReader = serviceProvider.GetService<IInputReader>();

            var inputPath = Path.Combine(@"C:\tmp", "input.txt");
            var outputPath = Path.Combine(@"C:\tmp", "output.txt");

            using (var fileStreamReader = File.OpenText(inputPath))
            {
                var data = inputReader.Parse(fileStreamReader);

                var procesor = serviceProvider.GetService<IDataProcessor>();
                var outputData = procesor.Process(data);

                var outputWriter = serviceProvider.GetService<IOutputWriter>();

                using (var outputStream = File.OpenWrite(outputPath))
                {
                    using (var streamWriter = new StreamWriter(outputStream))
                    {
                        outputWriter.Write(streamWriter, outputData);
                    }
                }
            }


            System.Console.ReadLine();
        }
    }
}
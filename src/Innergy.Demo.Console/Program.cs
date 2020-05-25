using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Innergy.Demo.Domain;
using Innergy.Demo.Services;
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

            var filePath = Path.Combine(@"C:\tmp", "input.txt");

            using (var fileStreamReader = File.OpenText(filePath))
            {
                inputReader.Parse(fileStreamReader);
                //System.Console.WriteLine(file.ReadToEnd());
            }

            System.Console.ReadLine();
        }
    }
}
using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Innergy.Demo.Domain;
using Innergy.Demo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Innergy.Demo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(new ServiceCollection());

            containerBuilder.RegisterType<TextFileInputReader>().As<IInputReader>();

            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            var inputReader = serviceProvider.GetService<IInputReader>();
            
            var filePath = Path.Combine(@"C:\tmp", "input.txt");

            using(var file = File.OpenText(filePath))
            {
                inputReader.Parse(file.BaseStream);
                //System.Console.WriteLine(file.ReadToEnd());
            }

            System.Console.ReadLine();
        }
    }
}

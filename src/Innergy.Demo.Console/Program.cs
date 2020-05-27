using Autofac;
using Autofac.Extensions.DependencyInjection;
using Innergy.Demo.Domain;
using Innergy.Demo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Console
{
    internal class Program
    {
        private const string DEFAULT_INPUT_FILE_PATH = @"\tmp\input.txt";

        private static void Main(string[] args)
        {
            var inputPath = args.Length > 0 ? args[0] : DEFAULT_INPUT_FILE_PATH;

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
                                         {
                                             builder.ClearProviders();
                                             builder.AddConsole();
                                         });

            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);

            builder.RegisterType<InputLineParser>().As<IInputLineParser>();
            builder.RegisterType<TextFileInputStrategy>().As<IInputStrategy>()
                   .WithParameter(new TypedParameter(typeof(string), inputPath));
            builder.RegisterType<DataProcessor>().As<IDataProcessor>();
            builder.RegisterType<InputLineModelBuilder>().As<IInputLineModelBuilder>();
            builder.RegisterType<DefaultOutputWriter>().As<IOutputWriter>();
            builder.RegisterType<DefaultOutputFormatter>().As<IOutputFormatter>();
            builder.RegisterType<DefaultOutputSorter>().As<IOutputSorter>();

            if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
            {
                builder.RegisterType<TextFileWriterStrategy>().As<IWriterStrategy>()
                       .WithParameter(new TypedParameter(typeof(string), args[1]));
            }
            else
            {
                builder.RegisterType<ConsoleWriterStrategy>().As<IWriterStrategy>();
            }

            builder.RegisterType<JobRunner>().As<IJobRunner>();

            var container = builder.Build();

            var jobRunner = container.Resolve<IJobRunner>();
            jobRunner.Run();
        }
    }
}
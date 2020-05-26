using Innergy.Demo.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Innergy.Demo.Services.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<IDataProcessor, DataProcessor>()
                                    .AddTransient<IInputLineParser, InputLineParser>()
                                    .AddTransient<IInputReader, TextFileInputReader>()
                                    .AddTransient<IOutputWriter, TextFileOutputWriter>()
                                    .AddTransient<IJobRunner, JobRunner>();
        }
    }
}
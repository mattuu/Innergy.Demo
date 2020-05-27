using System.Diagnostics.CodeAnalysis;
using Innergy.Demo.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Innergy.Demo.Services.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient<IDataProcessor, DataProcessor>()
                                    .AddTransient<IInputLineParser, InputLineParser>()
                                    .AddTransient<IInputLineModelBuilder, InputLineModelBuilder>()
                                    .AddTransient<IInputStrategy, TextFileInputStrategy>()
                                    .AddTransient<IOutputWriterStrategy, TextFileOutputWriterStrategy>()
                                    .AddTransient<IJobRunner, JobRunner>();
        }
    }
}
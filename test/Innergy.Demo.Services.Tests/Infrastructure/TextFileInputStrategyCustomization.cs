using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Innergy.Demo.Domain;
using Innergy.Demo.Services.Input;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Services.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class TextFileInputStrategyCustomization : ICustomization
    {
        private readonly string _filePath;

        public TextFileInputStrategyCustomization(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customize<TextFileInputStrategy>(cc =>
                                                         cc.FromFactory(() => new TextFileInputStrategy(fixture.Create<ILogger<TextFileInputStrategy>>(),
                                                                                                        fixture.Create<IInputLineParser>(),
                                                                                                        _filePath)));
        }
    }
}
using System.Collections.Generic;
using System.IO;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Moq;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class JobRunnerTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(JobRunner).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Run_ShouldLoadDataUsingInputStrategy([Frozen] Mock<IInputStrategy> inputStrategyMock, string source,
                                                         JobRunner sut)
        {
            // act
            using (var writer = new StreamWriter(new MemoryStream()))
            {
                sut.Run(source, writer);
            }

            // assert
            inputStrategyMock.Verify(m => m.Load(It.Is<string>(s => source.Equals(s))), Times.Once());
        }

        [Theory, AutoMoqData]
        public void Run_ShouldProcessData([Frozen] Mock<IInputStrategy> inputStrategyMock,
                                          [Frozen] Mock<IDataProcessor> dataProcessorMock,
                                          IEnumerable<InputLineModel> models,
                                          string source,
                                          JobRunner sut)
        {
            // arrange
            inputStrategyMock.Setup(m => m.Load(It.Is<string>(s => source.Equals(s)))).Returns(models);

            // act
            using (var writer = new StreamWriter(new MemoryStream()))
            {
                sut.Run(source, writer);
            }

            // assert
            dataProcessorMock.Verify(m => m.Process(It.Is<IEnumerable<InputLineModel>>(d => d == models)),
                                     Times.Once());
        }

        [Theory, AutoMoqData]
        public void Run_ShouldProduceOutputData([Frozen] Mock<IInputStrategy> inputStrategyMock,
                                                [Frozen] Mock<IDataProcessor> dataProcessorMock,
                                                [Frozen] Mock<IOutputWriter> outputWriterMock,
                                                IEnumerable<InputLineModel> models,
                                                IEnumerable<OutputGroupModel> outputModels,
                                                string source,
                                                JobRunner sut)
        {
            // arrange
            inputStrategyMock.Setup(m => m.Load(It.Is<string>(s => source.Equals(s)))).Returns(models);
            dataProcessorMock.Setup(m => m.Process(models)).Returns(outputModels);

            // act
            using (var writer = new StreamWriter(new MemoryStream()))
            {
                sut.Run(source, writer);
            }

            // assert
            outputWriterMock
               .Verify(m => m.Write(It.IsAny<TextWriter>(), It.Is<IEnumerable<OutputGroupModel>>(d => d == outputModels)),
                       Times.Once());
        }
    }
}
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
        public void Run_ShouldLoadDataUsingInputReader([Frozen] Mock<IInputReader> inputReaderMock, JobRunner sut)
        {
            // act
            using (var reader = new StreamReader(new MemoryStream()))
            {
                using (var writer = new StreamWriter(new MemoryStream()))
                {
                    sut.Run(reader, writer);
                }
            }

            // assert
            inputReaderMock.Verify(m => m.Parse(It.IsAny<StreamReader>()), Times.Once());
        }

        [Theory, AutoMoqData]
        public void Run_ShouldProcessData([Frozen] Mock<IInputReader> inputReaderMock,
                                          [Frozen] Mock<IDataProcessor> dataProcessorMock,
                                          IEnumerable<InputLineModel> models,
                                          JobRunner sut)
        {
            // arrange
            inputReaderMock.Setup(m => m.Parse(It.IsAny<StreamReader>())).Returns(models);

            // act
            using (var reader = new StreamReader(new MemoryStream()))
            {
                using (var writer = new StreamWriter(new MemoryStream()))
                {
                    sut.Run(reader, writer);
                }
            }

            // assert
            dataProcessorMock.Verify(m => m.Process(It.Is<IEnumerable<InputLineModel>>(d => d == models)),
                                     Times.Once());
        }

        [Theory, AutoMoqData]
        public void Run_ShouldProduceOutputData([Frozen] Mock<IInputReader> inputReaderMock,
                                                [Frozen] Mock<IDataProcessor> dataProcessorMock,
                                                [Frozen] Mock<IOutputWriter> outputWriterMock,
                                                IEnumerable<InputLineModel> models,
                                                IEnumerable<OutputGroupModel> outputModels,
                                                JobRunner sut)
        {
            // arrange
            inputReaderMock.Setup(m => m.Parse(It.IsAny<StreamReader>())).Returns(models);
            dataProcessorMock.Setup(m => m.Process(models)).Returns(outputModels);

            // act
            using (var reader = new StreamReader(new MemoryStream()))
            {
                using (var writer = new StreamWriter(new MemoryStream()))
                {
                    sut.Run(reader, writer);
                }
            }

            // assert
            outputWriterMock
               .Verify(m => m.Write(It.IsAny<TextWriter>(), It.Is<IEnumerable<OutputGroupModel>>(d => d == outputModels)),
                       Times.Once());
        }
    }
}
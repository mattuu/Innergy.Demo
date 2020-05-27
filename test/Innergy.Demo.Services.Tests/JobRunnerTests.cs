using System.Collections.Generic;
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
        public void Run_ShouldLoadDataUsingInputStrategy([Frozen] Mock<IInputStrategy> inputStrategyMock, JobRunner sut)
        {
            // act
            sut.Run();

            // assert
            inputStrategyMock.Verify(m => m.Load(), Times.Once());
        }

        [Theory, AutoMoqData]
        public void Run_ShouldProcessData([Frozen] Mock<IInputStrategy> inputStrategyMock,
                                          [Frozen] Mock<IDataProcessor> dataProcessorMock,
                                          IEnumerable<InputLineModel> models,
                                          JobRunner sut)
        {
            // arrange
            inputStrategyMock.Setup(m => m.Load()).Returns(models);

            // act
            sut.Run();

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
                                                JobRunner sut)
        {
            // arrange
            inputStrategyMock.Setup(m => m.Load()).Returns(models);
            dataProcessorMock.Setup(m => m.Process(models)).Returns(outputModels);

            // act
            sut.Run();

            // assert
            outputWriterMock.Verify(m => m.Write(It.Is<IEnumerable<OutputGroupModel>>(d => d == outputModels)),
                                    Times.Once());
        }
    }
}
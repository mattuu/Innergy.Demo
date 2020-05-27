using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Moq;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class DefaultOutputWriterTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(DefaultOutputWriter).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Write_ShouldSortItems([Frozen] Mock<IOutputSorter> outputSorterMock,
                                          IEnumerable<OutputGroupModel> models,
                                          DefaultOutputWriter sut)
        {
            // act
            sut.Write(models);

            // assert
            outputSorterMock.Verify(m => m.Sort(It.Is<IEnumerable<OutputGroupModel>>(d => d == models)), Times.Once());
        }

        [Theory, AutoMoqData]
        public void Write_ShouldFormatEachItem([Frozen] Mock<IOutputSorter> outputSorterMock,
                                               [Frozen] Mock<IOutputFormatter> outputFormatterMock,
                                               IEnumerable<OutputGroupModel> models,
                                               DefaultOutputWriter sut)
        {
            // arrange
            outputSorterMock.Setup(m => m.Sort(models)).Returns(models);

            // act
            sut.Write(models);

            // assert
            foreach (var outputGroupModel in models)
            {
                outputFormatterMock.Verify(m => m.FormatWarehouse(It.Is<OutputGroupModel>(d => d == outputGroupModel)),
                                           Times.Once());
            }
        }

        [Theory, AutoMoqData]
        public void Write_ShouldWriteEachFormattedItem([Frozen] Mock<IOutputSorter> outputSorterMock,
                                                       [Frozen] Mock<IOutputFormatter> outputFormatterMock,
                                                       [Frozen] Mock<IWriterStrategy> writerStrategyMock,
                                                       IEnumerable<Tuple<OutputGroupModel, string>> data,
                                                       DefaultOutputWriter sut)
        {
            // arrange
            var models = data.Select(d => d.Item1);
            outputSorterMock.Setup(m => m.Sort(models)).Returns(models);

            foreach (var item in data)
            {
                outputFormatterMock.Setup(m => m.FormatWarehouse(item.Item1)).Returns(item.Item2);
            }

            // act
            sut.Write(models);

            // assert
            foreach (var item in data)
            {
                writerStrategyMock.Verify(m => m.WriteLine(It.Is<string>(s => item.Item2.Equals(s))),
                                          Times.Once());
            }
        }
    }
}
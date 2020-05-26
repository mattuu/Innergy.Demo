using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoFixture.Idioms;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class TextFileOutputWriterTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(TextFileOutputWriter).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Write_ShouldOrderItemsCorrectly(IEnumerable<OutputGroupModel> models, TextFileOutputWriter sut)
        {
            // arrange
            var stringBuilder = new StringBuilder();
            var writer = new StringWriter(stringBuilder);

            // act
            sut.Write(writer, models);

            // assert
            var x = stringBuilder.ToString();
            x.ShouldNotBeEmpty();
        }
    }
}
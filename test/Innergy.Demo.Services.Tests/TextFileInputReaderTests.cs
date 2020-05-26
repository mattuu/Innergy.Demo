using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class TextFileInputReaderTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(TextFileInputReader).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Parse_ShouldProcessAllLines(IFixture fixture,
                                                [Frozen] Mock<IInputLineParser> inputLineParserMock,
                                                IEnumerable<string> lines,
                                                TextFileInputReader sut)
        {
            // arrange
            var ms = new MemoryStream();
            using (var sw = new StreamWriter(ms, leaveOpen: true))
            {
                foreach (var line in lines)
                {
                    sw.WriteLine(line);
                }
            }

            ms.Position = 0;

            inputLineParserMock.Setup(m => m.Parse(It.IsAny<string>())).Returns(fixture.Create<InputLineModel>());

            // act
            using (var reader = new StreamReader(ms))
            {
                var actual = sut.Parse(reader);

                // assert
                actual.Count().ShouldBe(lines.Count());
            }
        }


        [Theory, AutoMoqData]
        public void Parse_ShouldIgnoreCommentLines(IFixture fixture,
                                                   [Frozen] Mock<IInputLineParser> inputLineParserMock,
                                                   IEnumerable<string> lines,
                                                   string commentLine,
                                                   TextFileInputReader sut)
        {
            // arrange
            var ms = new MemoryStream();
            using (var sw = new StreamWriter(ms, leaveOpen: true))
            {
                foreach (var line in lines)
                {
                    sw.WriteLine(line);
                }

                sw.Write(commentLine);
            }

            ms.Position = 0;

            inputLineParserMock.Setup(m => m.Parse(It.IsAny<string>())).Returns(fixture.Create<InputLineModel>());

            inputLineParserMock.Setup(m => m.Parse(It.Is<string>(s => commentLine.Equals(s))))
                               .Returns(default(InputLineModel));

            // act
            using (var reader = new StreamReader(ms))
            {
                var actual = sut.Parse(reader);

                // assert
                actual.Count().ShouldBe(lines.Count());
            }
        }

        [Theory, AutoMoqData]
        public void Parse_ShouldIgnoreInvalidLines(IFixture fixture,
                                                   [Frozen] Mock<IInputLineParser> inputLineParserMock,
                                                   IEnumerable<string> lines,
                                                   string invalidLine,
                                                   TextFileInputReader sut)
        {
            // arrange
            var ms = new MemoryStream();
            using (var sw = new StreamWriter(ms, leaveOpen: true))
            {
                foreach (var line in lines)
                {
                    sw.WriteLine(line);
                }

                sw.Write(invalidLine);
            }

            ms.Position = 0;

            inputLineParserMock.Setup(m => m.Parse(It.IsAny<string>())).Returns(fixture.Create<InputLineModel>());

            inputLineParserMock.Setup(m => m.Parse(It.Is<string>(s => s.Equals(invalidLine))))
                               .Throws<InputLineParsingException>();

            // act
            using (var reader = new StreamReader(ms))
            {
                var actual = sut.Parse(reader);

                // assert
                actual.Count().ShouldBe(lines.Count());
            }
        }
    }
}
﻿using System.Collections.Generic;
using Xunit;
using Moq;
using Compiler.Parser;
using Compiler.Error;
using Compiler.Model;
using Compiler.Event;
using Compiler.Output;

namespace CompilerTest.Parser
{
    public class FreetextParserTest
    {
        private readonly FreetextParser parser;

        private readonly SectorElementCollection collection;

        private readonly Mock<IEventLogger> log;

        public FreetextParserTest()
        {
            this.log = new Mock<IEventLogger>();
            this.collection = new SectorElementCollection();
            this.parser = (FreetextParser)(new SectionParserFactory(this.collection, this.log.Object))
                .GetParserForSection(OutputSections.ESE_FREETEXT);
        }

        [Fact]
        public void TestItRaisesASyntaxErrorIfIncorrectNumberOfSegments()
        {
            SectorFormatData data = new SectorFormatData(
                "test.txt",
                "test",
                "test",
                new List<string>(new string[] { "abc:def:ghi" })
            );

            this.parser.ParseData(data);
            this.log.Verify(foo => foo.AddEvent(It.IsAny<SyntaxError>()), Times.Once);
        }

        [Fact]
        public void TestItRaisesAnErrorIfInvalidCoordinate()
        {
            SectorFormatData data = new SectorFormatData(
                "test.txt",
                "test",
                "test",
                new List<string>(new string[] { "abc:def:Title:Text" })
            );

            this.parser.ParseData(data);
            this.log.Verify(foo => foo.AddEvent(It.IsAny<SyntaxError>()), Times.Once);
        }

        [Fact]
        public void TestItHandlesMetadata()
        {
            SectorFormatData data = new SectorFormatData(
                "test.txt",
                "test",
                "test",
                new List<string>(new string[] { "" })
            );

            this.parser.ParseData(data);
            Assert.IsType<BlankLine>(this.collection.Compilables[OutputSections.ESE_FREETEXT][0]);
        }

        [Fact]
        public void TestItAddsFreetextData()
        {
            SectorFormatData data = new SectorFormatData(
                "test.txt",
                "test",
                "test",
                new List<string>(new string[] { "N054.28.46.319:W006.15.33.933:Title:Text ;comment" })
            );

            this.parser.ParseData(data);

            Freetext result = this.collection.Freetext[0];
            Assert.Equal(new Coordinate("N054.28.46.319", "W006.15.33.933"), result.Coordinate);
            Assert.Equal("Title", result.Title);
            Assert.Equal("Text", result.Text);
            Assert.Equal("comment", result.Comment);
        }
    }
}
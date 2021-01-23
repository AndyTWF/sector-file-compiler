﻿using System.Collections.Generic;
using Xunit;
using Moq;
using Compiler.Error;
using Compiler.Model;
using Compiler.Input;

namespace CompilerTest.Parser
{
    public class ArtccParserTest: AbstractParserTestCase
    {
        public static IEnumerable<object[]> BadData => new List<object[]>
        {
            new object[] { new List<string>{
                "EGTT London FIR"
            }}, // Too few segments
            new object[] { new List<string>{
                "EGTT London FIR N050.57.00.000 W001.21.24.490 N050.57.00.000 N001.21.24.490"
            }}, // Invalid end point
            new object[] { new List<string>{
                "EGTT London FIR N050.57.00.000 N001.21.24.490 N050.57.00.000 W001.21.24.490"
            }}, // Invalid start point
        };

        [Theory]
        [MemberData(nameof(BadData))]
        public void ItRaisesSyntaxErrorsOnBadData(List<string> lines)
        {
            this.RunParserOnLines(lines);

            Assert.Empty(this.sectorElementCollection.Artccs);
            Assert.Empty(this.sectorElementCollection.HighArtccs);
            Assert.Empty(this.sectorElementCollection.LowArtccs);
            this.logger.Verify(foo => foo.AddEvent(It.IsAny<SyntaxError>()), Times.Once);
        }

        [Fact]
        public void TestItAddsArtccData()
        {
            this.RunParserOnLines(
                new List<string>(new[] { "EGTT London FIR   N050.57.00.001 W001.21.24.490 N050.57.00.002 W001.21.24.490;comment" })
            );

            ArtccSegment result = this.sectorElementCollection.Artccs[0];
            Assert.Equal("EGTT London FIR", result.Identifier);
            Assert.Equal(ArtccType.REGULAR, result.Type);
            Assert.Equal(new Point(new Coordinate("N050.57.00.001", "W001.21.24.490")), result.StartPoint);
            Assert.Equal(new Point(new Coordinate("N050.57.00.002", "W001.21.24.490")), result.EndPoint);
            this.AssertExpectedMetadata(result);
        }

        [Fact]
        public void TestItAddsArtccDataWithIdentifiers()
        {
            this.RunParserOnLines(
                new List<string>(new[] { "EGTT London FIR   DIKAS DIKAS BHD BHD;comment" })
            );

            ArtccSegment result = this.sectorElementCollection.Artccs[0];
            Assert.Equal("EGTT London FIR", result.Identifier);
            Assert.Equal(ArtccType.REGULAR, result.Type);
            Assert.Equal(new Point("DIKAS"), result.StartPoint);
            Assert.Equal(new Point("BHD"), result.EndPoint);
            this.AssertExpectedMetadata(result);
        }

        protected override InputDataType GetInputDataType()
        {
            return InputDataType.SCT_ARTCC;
        }
    }
}

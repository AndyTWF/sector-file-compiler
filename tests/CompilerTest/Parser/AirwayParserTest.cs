﻿using System.Collections.Generic;
using Xunit;
using Moq;
using Compiler.Error;
using Compiler.Model;
using Compiler.Input;

namespace CompilerTest.Parser
{
    public class AirwayParserTest: AbstractParserTestCase
    {
        public static IEnumerable<object[]> BadData => new List<object[]>
        {
            new object[] { new List<string>{
                "BHD DIKAS DIKAS"
            }}, // Too few segments
            new object[] { new List<string>{
                "BHD BHD DIKAS DIKAS EXMOR"
            }}, // Too many segments
            new object[] { new List<string>{
                "N050.57.00.000 W001.21.24.490 N050.57.00.000 N001.21.24.490"
            }}, // Invalid end point
            new object[] { new List<string>{
                "N050.57.00.000 N001.21.24.490 N050.57.00.000 W001.21.24.490"
            }}, // Invalid start point
        };

        [Theory]
        [MemberData(nameof(BadData))]
        public void ItRaisesSyntaxErrorsOnBadData(List<string> lines)
        {
            RunParserOnLines(lines);

            Assert.Empty(sectorElementCollection.HighAirways);
            Assert.Empty(sectorElementCollection.LowAirways);
            logger.Verify(foo => foo.AddEvent(It.IsAny<SyntaxError>()), Times.Once);
        }
        
        [Fact]
        public void TestItAddsAirwayData()
        {
            RunParserOnLines(new List<string>(new[] { "N050.57.00.001 W001.21.24.490 N050.57.00.002 W001.21.24.490;comment" }));

            AirwaySegment result = sectorElementCollection.LowAirways[0];
            Assert.Equal("TEST", result.Identifier);
            Assert.Equal(AirwayType.LOW, result.Type);
            Assert.Equal(new Point(new Coordinate("N050.57.00.001", "W001.21.24.490")), result.StartPoint);
            Assert.Equal(new Point(new Coordinate("N050.57.00.002", "W001.21.24.490")), result.EndPoint);
            AssertExpectedMetadata(result);
        }

        [Fact]
        public void TestItAddsAirwayDataWithIdentifiers()
        {
            RunParserOnLines(new List<string>(new[] { "DIKAS DIKAS BHD BHD;comment" }));
            
            AirwaySegment result = sectorElementCollection.LowAirways[0];
            Assert.Equal("TEST", result.Identifier);
            Assert.Equal(AirwayType.LOW, result.Type);
            Assert.Equal(new Point("DIKAS"), result.StartPoint);
            Assert.Equal(new Point("BHD"), result.EndPoint);
            AssertExpectedMetadata(result);
        }

        protected override InputDataType GetInputDataType()
        {
            return InputDataType.SCT_LOWER_AIRWAYS;
        }
    }
}

﻿using System.Collections.Generic;
using Xunit;
using Moq;
using Compiler.Error;
using Compiler.Model;
using Compiler.Input;

namespace CompilerTest.Parser
{
    public class RunwayParserTest: AbstractParserTestCase
    {
        public static IEnumerable<object[]> BadData => new List<object[]>
        {
            new object[] { new List<string>{
                "15 33 148 N052.27.48.520 W001.45.31.430 N052.26.46.580 W001.44.22.560 ;comment"
            }}, // Too few segments
            new object[] { new List<string>{
                "37R 33 148 328 N052.27.48.520 W001.45.31.430 N052.26.46.580 W001.44.22.560 ;comment"
            }}, // Invalid first identifier
            new object[] { new List<string>{
                "15 00A 148 328 N052.27.48.520 W001.45.31.430 N052.26.46.580 W001.44.22.560 ;comment"
            }}, // Invalid reverse identifier
            new object[] { new List<string>{
                "15 33 abc 328 N052.27.48.520 W001.45.31.430 N052.26.46.580 W001.44.22.560 ;comment"
            }}, // Invalid first heading
            new object[] { new List<string>{
                "15 33 148 360 N052.27.48.520 W001.45.31.430 N052.26.46.580 W001.44.22.560 ;comment"
            }}, // Invalid reverse heading
            new object[] { new List<string>{
                "15 33 148 328 abc W001.45.31.430 N052.26.46.580 W001.44.22.560 ;comment"
            }}, // Invalid first coordinate
            new object[] { new List<string>{
                "15 33 148 328 N052.27.48.520 W001.45.31.430 N052.26.46.580 abc ;comment"
            }}, // Invalid second coordinate
        };

        [Theory]
        [MemberData(nameof(BadData))]
        public void ItRaisesSyntaxErrorsOnBadData(List<string> lines)
        {
            RunParserOnLines(lines);

            Assert.Empty(sectorElementCollection.Runways);
            logger.Verify(foo => foo.AddEvent(It.IsAny<SyntaxError>()), Times.Once);
        }

        [Fact]
        public void TestItAddsRunwayDataWithDescription()
        {
            RunParserOnLines(new List<string>(new[] { "15 33 148 328 N052.27.48.520 W001.45.31.430 N052.26.46.580 W001.44.22.560 ;comment" }));

            Runway result = sectorElementCollection.Runways[0];
            Assert.Equal("15", result.FirstIdentifier);
            Assert.Equal(148, result.FirstHeading);
            Assert.Equal(new Coordinate("N052.27.48.520", "W001.45.31.430"), result.FirstThreshold);
            Assert.Equal("33", result.ReverseIdentifier);
            Assert.Equal(328, result.ReverseHeading);
            Assert.Equal(new Coordinate("N052.26.46.580", "W001.44.22.560"), result.ReverseThreshold);
            Assert.Equal("TESTFOLDER", result.AirfieldIcao);
            AssertExpectedMetadata(result);
        }

        protected override InputDataType GetInputDataType()
        {
            return InputDataType.SCT_RUNWAYS;
        }
    }
}

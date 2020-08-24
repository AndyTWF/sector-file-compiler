﻿using Xunit;
using Compiler.Model;

namespace CompilerTest.Model
{
    public class GeoSegmentTest
    {
        private GeoSegment segment;

        public GeoSegmentTest()
        {
            this.segment = new GeoSegment(
                new Point(new Coordinate("abc", "def")),
                new Point(new Coordinate("ghi", "jkl")),
                "red",
                "comment"
            );
        }

        [Fact]
        public void TestItSetsFirstCoordinate()
        {
            Assert.Equal(new Point(new Coordinate("abc", "def")), this.segment.FirstPoint);
        }

        [Fact]
        public void TestItSetsSecondCoordinate()
        {
            Assert.Equal(new Point(new Coordinate("ghi", "jkl")), this.segment.SecondPoint);
        }

        [Fact]
        public void TestItSetsColour()
        {
            Assert.Equal("red", this.segment.Colour);
        }

        [Fact]
        public void TestItCompilesWithComment()
        {
            Assert.Equal("abc def ghi jkl red ;comment\r\n", this.segment.Compile());
        }

        [Fact]
        public void TestItCompilesWithNoComment()
        {
            GeoSegment segment = new GeoSegment(
                new Point(new Coordinate("abc", "def")),
                new Point(new Coordinate("ghi", "jkl")),
                "red",
                ""
            );
            Assert.Equal("abc def ghi jkl red\r\n", segment.Compile());
        }
    }
}
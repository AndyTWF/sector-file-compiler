﻿using Xunit;
using Compiler.Model;

namespace CompilerTest.Model
{
    public class PointTest
    {
        [Fact]
        public void TestItHasTypeIdentifier()
        {
            Point point = new Point("TESTF");
            Assert.Equal(Point.TypeIdentifier, point.Type());
        }

        [Fact]
        public void TestItCompilesIdentifier()
        {
            Point point = new Point("TESTF");
            Assert.Equal("TESTF TESTF", point.ToString());
        }

        [Fact]
        public void TestItHasTypeCoordinate()
        {
            Point point = new Point(new Coordinate("abc", "def"));
            Assert.Equal(Point.TypeCoordinate, point.Type());
        }

        [Fact]
        public void TestItCompilesCoordinate()
        {
            Point point = new Point(new Coordinate("abc", "def"));
            Assert.Equal("abc def", point.ToString());
        }
    }
}

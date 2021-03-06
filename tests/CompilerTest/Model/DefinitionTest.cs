﻿using Compiler.Model;
using Xunit;

namespace CompilerTest.Model
{
    public class DefinitionTest
    {
        [Theory]
        [InlineData("Test.txt", 1, "Test.txt", 1, true)]  // All the same
        [InlineData("Test1.txt", 1, "Test.txt", 1, false)]  // Different first filename
        [InlineData("Test.txt", 2, "Test.txt", 1, false)]  // Different first line number
        [InlineData("Test.txt", 1, "Test2.txt", 1, false)]  // Different second filename
        [InlineData("Test.txt", 1, "Test.txt", 2, false)]  // Different second line number
        public void TestItChecksEquality(
            string firstFilename,
            int firstLineNumber, 
            string secondFileName,
            int secondLineNumber,
            bool expectedEquality
        ) {
            Assert.Equal(
                expectedEquality,
                new Definition(firstFilename, firstLineNumber).Equals(new Definition(secondFileName, secondLineNumber))
            );
        }

        [Fact]
        public void TestItHasAStringRepresentation()
        {
            Assert.Equal("Test1.txt on line 23", new Definition("Test1.txt", 23).ToString());
        }
    }
}

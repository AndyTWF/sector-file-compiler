﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Input;
using CompilerTest.Mock;
using Xunit;

namespace CompilerTest.Input
{
    public class SectorDataFileFactoryTest
    {
        private readonly SectorDataFileFactory factory;
        
        public SectorDataFileFactoryTest()
        {
            this.factory = new(new MockInputStreamFactory(new List<string>()));
        }

        [Fact]
        public void TestItReturnsCorrectTypeForHeaders()
        {
            Assert.IsType<HeaderDataFile>(this.factory.Create("test", InputDataType.FILE_HEADERS));
        }
        
        [Fact]
        public void TestItReturnsCorrectTypeOtherFiles()
        {
            Assert.IsType<SectorDataFile>(this.factory.Create("test", InputDataType.ESE_AGREEMENTS));
        }
    }
}
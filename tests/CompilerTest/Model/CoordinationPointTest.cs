﻿using Xunit;
using Compiler.Model;
using CompilerTest.Bogus.Factory;

namespace CompilerTest.Model
{
    public class CoordinationPointTest
    {
        private readonly CoordinationPoint model;

        public CoordinationPointTest()
        {
            this.model = new CoordinationPoint(
                false,
                "*",
                "*",
                "ABTUM",
                "EGKK",
                "26L",
                "TCE",
                "TCSW",
                "*",
                "14000",
                "ABTUMDES",
                DefinitionFactory.Make(),
                DocblockFactory.Make(),
                CommentFactory.Make()
            );
        }

        [Fact]
        public void TestItSetsIsFirCopy()
        {
            Assert.False(this.model.IsFirCopx);
        }

        [Fact]
        public void TestItSetsDepartureAirportOrFixBefore()
        {
            Assert.Equal("*", this.model.DepartureAirportOrFixBefore);
        }

        [Fact]
        public void TestItSetsDepartureRunway()
        {
            Assert.Equal("*", this.model.DepartureRunway);
        }

        [Fact]
        public void TestItSetsCoordinationFix()
        {
            Assert.Equal("ABTUM", this.model.CoordinationFix);
        }

        [Fact]
        public void TestItSetsArrivalAirportOrFixAfter()
        {
            Assert.Equal("EGKK", this.model.ArrivalAirportOrFixAfter);
        }

        [Fact]
        public void TestItSetsArrivalRunway()
        {
            Assert.Equal("26L", this.model.ArrivalRunway);
        }

        [Fact]
        public void TestItSetsFromSector()
        {
            Assert.Equal("TCE", this.model.FromSector);
        }

        [Fact]
        public void TestItSetsToSector()
        {
            Assert.Equal("TCSW", this.model.ToSector);
        }

        [Fact]
        public void TestItSetsClimbLevel()
        {
            Assert.Equal("*", this.model.ClimbLevel);
        }

        [Fact]
        public void TestItSetsDescendLevel()
        {
            Assert.Equal("14000", this.model.DescendLevel);
        }

        [Fact]
        public void TestItSetsName()
        {
            Assert.Equal("ABTUMDES", this.model.Name);
        }

        [Fact]
        public void TestItCompiles()
        {
            Assert.Equal(
                "COPX:*:*:ABTUM:EGKK:26L:TCE:TCSW:*:14000:ABTUMDES",
                this.model.GetCompileData(new SectorElementCollection())
            );
        }

        [Fact]
        public void TestItCompilesFirCopx()
        {
            CoordinationPoint model2 = new(
                true,
                "*",
                "*",
                "ABTUM",
                "EGKK",
                "26L",
                "TCE",
                "TCSW",
                "*",
                "14000",
                "ABTUMDES",
                DefinitionFactory.Make(),
                DocblockFactory.Make(),
                CommentFactory.Make()
            );

            Assert.Equal(
                "FIR_COPX:*:*:ABTUM:EGKK:26L:TCE:TCSW:*:14000:ABTUMDES",
                model2.GetCompileData(new SectorElementCollection())
            );
        }
    }
}
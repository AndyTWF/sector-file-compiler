﻿using Compiler.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Compiler.Output
{
    public class HighAirwaysCollector : ICompilableElementCollector
    {
        private readonly SectorElementCollection sectorElements;
        private readonly OutputGroupRepository repository;

        public HighAirwaysCollector(SectorElementCollection sectorElements, OutputGroupRepository repository)
        {
            this.sectorElements = sectorElements;
            this.repository = repository;
        }

        public IEnumerable<IGrouping<OutputGroup, ICompilableElementProvider>> GetCompilableElements()
        {
            return this.sectorElements.LowAirways.GroupBy(
                airway => this.repository.GetForDefinitionFile(airway.GetDefinition()),
                airway => airway
            );
        }
    }
}
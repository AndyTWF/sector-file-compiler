﻿using System.Collections.Generic;
using System.Linq;
using Compiler.Model;

namespace Compiler.Collector
{
    public class RadarCollector : ICompilableElementCollector
    {
        private readonly SectorElementCollection sectorElements;

        public RadarCollector(SectorElementCollection sectorElements)
        {
            this.sectorElements = sectorElements;
        }

        public IEnumerable<ICompilableElementProvider> GetCompilableElements()
        {
            return sectorElements.Radars.Concat<ICompilableElementProvider>(sectorElements.RadarHoles);
        }
    }
}

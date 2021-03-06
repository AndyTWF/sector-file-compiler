﻿using System.Collections.Generic;
using Compiler.Model;

namespace Compiler.Collector
{
    /*
     * An interface that collects compilable elements from the SectorElementCollection
     * and sorts them ready for output.
     */
    public interface ICompilableElementCollector
    {
        public IEnumerable<ICompilableElementProvider> GetCompilableElements();
    }
}

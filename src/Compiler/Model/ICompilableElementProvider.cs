﻿using System.Collections.Generic;

namespace Compiler.Model
{
    /*
     * Interface for classes that provide a collection of elements that can
     * be individually compiled.
     */
    public interface ICompilableElementProvider
    {
        public IEnumerable<ICompilableElement> GetCompilableElements();
    }
}

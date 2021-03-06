﻿using Compiler.Output;

namespace Compiler.Model
{
    public class HeaderLine: ICompilableElement
    {
        public Comment Line { get; }
        private readonly Definition definition;

        public HeaderLine(Comment line, Definition definition)
        {
            this.Line = line;
            this.definition = definition;
        }

        public Definition GetDefinition()
        {
            return this.definition;
        }

        public void Compile(SectorElementCollection elements, IOutputWriter output)
        {
            output.WriteLine(Line.ToString());
        }
    }
}

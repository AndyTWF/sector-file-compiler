﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Model
{
    public class InfoMagneticVariation : AbstractCompilableElement
    {
        public InfoMagneticVariation(
            double variation,
            Definition definition,
            Docblock docblock,
            Comment inlineComment
        ) : base(definition, docblock, inlineComment)
        {
            this.Variation = variation;
        }

        public double Variation { get; }

        public override string GetCompileData()
        {
            return this.Variation.ToString("n1");
        }
    }
}
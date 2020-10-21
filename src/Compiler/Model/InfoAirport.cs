﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Model
{
    public class InfoAirport : AbstractCompilableElement
    {
        public InfoAirport(string airportIcao, Definition definition, Docblock docblock, Comment inlineComment)
            : base(definition, docblock, inlineComment)
        {
            this.AirportIcao = airportIcao;
        }

        public string AirportIcao { get; }

        public override string GetCompileData()
        {
            return this.AirportIcao;
        }
    }
}

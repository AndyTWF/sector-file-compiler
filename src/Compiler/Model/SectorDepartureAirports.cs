﻿using System.Collections.Generic;

namespace Compiler.Model
{
    /*
     * Represents a single DEPAPT definition under each SECTOR definition
     */
    public class SectorDepartureAirports : AbstractCompilableElement
    {
        public SectorDepartureAirports(
            List<string> airports,
            Definition definition,
            Docblock docblock,
            Comment inlineComment
        )
            : base(definition, docblock, inlineComment)
        {
            this.Airports = airports;
        }

        public List<string> Airports { get; }

        public override bool Equals(object obj)
        {
            if (
                !(obj is SectorArrivalAirports) ||
                (obj as SectorDepartureAirports).Airports.Count != this.Airports.Count
            ) {
                return false;
            }

            for (int i = 0; i < this.Airports.Count; i++)
            {
                if (this.Airports[i] != (obj as SectorDepartureAirports).Airports[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string GetCompileData(SectorElementCollection elements)
        {
            return $"DEPAPT:{string.Join(":", this.Airports)}";
        }
    }
}

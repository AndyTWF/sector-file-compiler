﻿using System.Collections.Generic;
using Compiler.Model;
using Compiler.Error;
using Compiler.Input;
using Compiler.Event;
using Compiler.Validate;
using System;

namespace Compiler.Parser
{
    public class SectorParser : ISectorDataParser
    {
        private readonly SectorElementCollection sectorElements;
        private readonly IEventLogger errorLog;

        public SectorParser(
            SectorElementCollection sectorElements,
            IEventLogger errorLog
        ) {

            this.sectorElements = sectorElements;
            this.errorLog = errorLog;
        }

        public void ParseData(AbstractSectorDataFile data)
        {
            bool checkedFirstLine = false;

            int minimumAltitude = 0;
            int maximumAltitude = 0;
            SectorOwnerHierarchy ownerHierarchy = null;
            List<SectorAlternateOwnerHierarchy> altOwners = new List<SectorAlternateOwnerHierarchy>();
            SectorBorder border = new SectorBorder();
            List<SectorActive> actives = new List<SectorActive>();
            List<SectorGuest> guests = new List<SectorGuest>();
            SectorDepartureAirports departureAirports = new SectorDepartureAirports();
            SectorArrivalAirports arrivalAirports = new SectorArrivalAirports();
            SectorData declarationLine = new SectorData();
            foreach (SectorData line in data)
            {
                if (!checkedFirstLine)
                {
                    declarationLine = line;
                    // Check the declaration line
                    if (declarationLine.dataSegments[0] != "SECTOR")
                    {
                        this.errorLog.AddEvent(
                            new SyntaxError("Invalid SECTOR declaration", declarationLine)
                        );
                        return;
                    }

                    // Check the minimum and maximum altitudes
                    if (!int.TryParse(declarationLine.dataSegments[2], out minimumAltitude))
                    {
                        this.errorLog.AddEvent(
                            new SyntaxError("SECTOR minimum altitude must be an integer", declarationLine)
                        );
                    }

                    if (!int.TryParse(declarationLine.dataSegments[3], out maximumAltitude))
                    {
                        this.errorLog.AddEvent(
                            new SyntaxError("SECTOR maximum altitude must be an integer", declarationLine)
                        );
                    }
                    checkedFirstLine = true;
                    continue;
                }

                try
                {
                    switch (line.dataSegments[0])
                    {
                        case "OWNER":
                            ownerHierarchy = this.ParseOwnerHierarchy(line);
                            break;
                        case "ALTOWNER":
                            altOwners.Add(this.ParseAlternateOwnerHierarchy(line));
                            break;
                        case "BORDER":
                            if (border.BorderLines.Count != 0)
                            {
                                this.errorLog.AddEvent(
                                    new SyntaxError("Each SECTOR declaration may only have one BORDER", line)
                                );
                                return;
                            }
                            border = this.ParseBorder(line);
                            break;
                        case "ACTIVE":
                            actives.Add(this.ParseActive(line));
                            break;
                        case "GUEST":
                            guests.Add(this.ParseGuest(line));
                            break;
                        case "DEPAPT":
                            if (departureAirports.Airports.Count != 0)
                            {
                                this.errorLog.AddEvent(
                                     new SyntaxError("Each SECTOR declaration may only have one DEPAPT definition", line)
                                );
                                return;
                            }

                            departureAirports = this.ParseDepartureAirport(line);
                            break;
                        case "ARRAPT":
                            if (arrivalAirports.Airports.Count != 0)
                            {
                                this.errorLog.AddEvent(
                                     new SyntaxError("Each SECTOR declaration may only have one ARRAPT definition", line)
                                );
                                return;
                            }

                            arrivalAirports = this.ParseArrivalAirport(line);
                            break;
                        default:
                            this.errorLog.AddEvent(
                                 new SyntaxError("Unknown SECTOR line type", line)
                            );
                            return;
                    }
                } catch (ArgumentException exception) {
                    this.errorLog.AddEvent(
                        new SyntaxError(exception.Message, line)
                    );
                    return;
                }

            }

            if (ownerHierarchy == null)
            {
                this.errorLog.AddEvent(
                    new SyntaxError("Every SECTOR must have an owner", declarationLine)
                );
                this.errorLog.AddEvent(
                    new ParserSuggestion("Have you added an OWNER declaration?")
                );
                return;
            }

            this.sectorElements.Add(
                new Sector(
                    declarationLine.dataSegments[1],
                    minimumAltitude,
                    maximumAltitude,
                    ownerHierarchy,
                    altOwners,
                    actives,
                    guests,
                    border,
                    arrivalAirports,
                    departureAirports,
                    declarationLine.definition,
                    declarationLine.docblock,
                    declarationLine.inlineComment
                )
            );
        }

        /*
         * Parse and validate an OWNER line
         */
        private SectorOwnerHierarchy ParseOwnerHierarchy(SectorData line)
        {
            if (line.dataSegments.Count < 2)
            {
                throw new ArgumentException("Invalid number of OWNER segements");
            }

            List<string> owners = new List<string>();

            for (int i = 1; i < line.dataSegments.Count; i++)
            {
                owners.Add(line.dataSegments[i]);
            }

            return new SectorOwnerHierarchy(
                owners,
                line.definition,
                line.docblock,
                line.inlineComment
            );
        }

        /*
         * Parse and valdiate an ALTOWNER line
         */
        private SectorAlternateOwnerHierarchy ParseAlternateOwnerHierarchy(SectorData line)
        {
            if (line.dataSegments.Count < 3)
            {
                throw new ArgumentException("Invalid number of ALTOWNER segements");
            }

            List<string> altOwners = new List<string>();
            for (int i = 2; i < line.dataSegments.Count; i++)
            {
                altOwners.Add(line.dataSegments[i]);
            }

            return new SectorAlternateOwnerHierarchy(
                line.dataSegments[1],
                altOwners,
                line.definition,
                line.docblock,
                line.inlineComment
            );
        }

        /*
         * Parse and valdiate a BORDER line
         */
        private SectorBorder ParseBorder(SectorData line)
        {
            if (line.dataSegments.Count < 2)
            {
                throw new ArgumentException("Invalid number of BORDER segements");
            }

            List<string> borders = new List<string>();
            for (int i = 1; i < line.dataSegments.Count; i++)
            {
                borders.Add(line.dataSegments[i]);
            }

            return new SectorBorder(
                borders,
                line.definition,
                line.docblock,
                line.inlineComment
            );
        }

        /*
         * Parse and valdiate a ACTIVE line
         */
        private SectorActive ParseActive(SectorData line)
        {
            if (line.dataSegments.Count != 3)
            {
                throw new ArgumentException("Invalid number of ACTIVE segements ");
            }

            if (!AirportValidator.ValidEuroscopeAirport(line.dataSegments[1]))
            {
                throw new ArgumentException("Invalid airport designator in ACTIVE segement ");
            }

            if (!RunwayValidator.RunwayValidIncludingAdjacent(line.dataSegments[2]))
            {
                throw new ArgumentException("Invalid runway designator in ACTIVE segement ");
            }

            return new SectorActive(
                line.dataSegments[1],
                line.dataSegments[2],
                line.definition,
                line.docblock,
                line.inlineComment
            );
        }

        /*
        * Parse and valdiate a GUEST line
        */
        private SectorGuest ParseGuest(SectorData line)
        {
            if (line.dataSegments.Count != 4)
            {
                throw new ArgumentException("Invalid number of GUEST segements ");
            }

            if (!AirportValidator.ValidSectorGuestAirport(line.dataSegments[2]))
            {
                throw new ArgumentOutOfRangeException("Invalid departure airport designator in GUEST segement ");
            }

            if (!AirportValidator.ValidSectorGuestAirport(line.dataSegments[3]))
            {
                throw new ArgumentException("Invalid arrival airport designator in GUEST segement ");
            }

            return new SectorGuest(
                line.dataSegments[1],
                line.dataSegments[2],
                line.dataSegments[3],
                line.definition,
                line.docblock,
                line.inlineComment
            );
        }

        /*
        * Parse and valdiate a DEPAPT line
        */
        private SectorDepartureAirports ParseDepartureAirport(SectorData line)
        {
            if (line.dataSegments.Count < 2)
            {
                throw new ArgumentException("Invalid number of DEPAPT segments ");
            }

            List<string> airports = new List<string>();
            for (int i = 1; i < line.dataSegments.Count; i++)
            {
                if (!AirportValidator.IcaoValid(line.dataSegments[i]))
                {
                    throw new ArgumentException("Invalid ICAO code in DEPAPT ");
                }

                airports.Add(line.dataSegments[i]);
            }

            return new SectorDepartureAirports(
                airports,
                line.definition,
                line.docblock,
                line.inlineComment
            );
        }

        /*
        * Parse and valdiate a DEPAPT line
        */
        private SectorArrivalAirports ParseArrivalAirport(SectorData line)
        {
            if (line.dataSegments.Count < 2)
            {
                throw new ArgumentException("Invalid number of ARRAPT segments ");
            }

            List<string> airports = new List<string>();
            for (int i = 1; i < line.dataSegments.Count; i++)
            {
                if (!AirportValidator.IcaoValid(line.dataSegments[i]))
                {
                    throw new ArgumentException("Invalid ICAO code in ARRAPT ");
                }

                airports.Add(line.dataSegments[i]);
            }

            return new SectorArrivalAirports(
                airports,
                line.definition,
                line.docblock,
                line.inlineComment
            );
        }
    }
}

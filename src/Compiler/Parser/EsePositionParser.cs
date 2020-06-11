﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Compiler.Model;
using Compiler.Error;
using Compiler.Event;

namespace Compiler.Parser
{
    public class EsePositionParser: AbstractSectorElementParser
    {
        private readonly ISectorLineParser sectorLineParser;
        private readonly IFrequencyParser frequencyParser;
        private readonly SectorElementCollection sectorElements;
        private readonly IEventLogger errorLog;
        private readonly Regex squawkRegex;

        private readonly List<string> allowedTypes = new List<string>()
        {
            "OBS",
            "DEL",
            "GND",
            "APP",
            "TWR",
            "CTR",
            "FSS",
            "ATIS",
            "INFO"
        };

        const string noData = "-";

        public EsePositionParser(
            MetadataParser metadataParser,
            ISectorLineParser sectorLineParser,
            IFrequencyParser frequencyParser,
            SectorElementCollection sectorElements,
            IEventLogger errorLog
        ) : base(metadataParser)
        {
            this.sectorLineParser = sectorLineParser;
            this.frequencyParser = frequencyParser;
            this.sectorElements = sectorElements;
            this.errorLog = errorLog;
            this.squawkRegex = new Regex(@"[0-7]{4}");
        }

        public override void ParseData(SectorFormatData data)
        {
            for (int i = 0; i < data.lines.Count; i++)
            {
                // Defer all metadata lines to the base
                if (this.ParseMetadata(data.lines[i]))
                {
                    continue;
                }

                SectorFormatLine sectorData = this.sectorLineParser.ParseLine(data.lines[i]);
                if (sectorData.dataSegments.Count < 11)
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Incorrect number of ESE position segments", data.fullPath, i + 1)
                    );
                    this.errorLog.AddEvent(
                        new ParserSuggestion("Have you remembered to add the two non-used fields?")
                    );
                    continue;
                }

                if (this.frequencyParser.ParseFrequency(sectorData.dataSegments[2]) == null)
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Invalid RTF frequency " + sectorData.dataSegments[0], data.fullPath, i + 1)
                    );
                    continue;
                }

                if (!this.allowedTypes.Contains(sectorData.dataSegments[6]))
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("Unknown Position suffix " + sectorData.dataSegments[0], data.fullPath, i + 1)
                    );
                    continue;
                }

                if (sectorData.dataSegments[9] != EsePositionParser.noData && sectorData.dataSegments[9] != "")
                {
                    if (!squawkRegex.IsMatch(sectorData.dataSegments[9])) {
                        this.errorLog.AddEvent(
                            new SyntaxError("Invalid squawk range start " + sectorData.dataSegments[0], data.fullPath, i + 1)
                        );
                        continue;
                    }

                    if (!squawkRegex.IsMatch(sectorData.dataSegments[10]))
                    {
                        this.errorLog.AddEvent(
                            new SyntaxError("Invalid squawk range end " + sectorData.dataSegments[0], data.fullPath, i + 1)
                        );
                        continue;
                    }

                    if (int.Parse(sectorData.dataSegments[10]) < int.Parse(sectorData.dataSegments[9]))
                    {
                        this.errorLog.AddEvent(
                            new SyntaxError("Squawk range end is smaller than start " + sectorData.dataSegments[0], data.fullPath, i + 1)
                        );
                        continue;
                    }
                }

                // Coordinates start at position 11
                int coordNumber = 11;

                // Check if there's too many vis centers
                if (sectorData.dataSegments.Count - coordNumber > 8)
                {
                    this.errorLog.AddEvent(
                        new SyntaxError("A maxium of 4 visibility centers may be specified " + sectorData.dataSegments[0], data.fullPath, i + 1)
                    );
                }

                bool coordinateError = false;
                List<Coordinate> parsedCoordinates = new List<Coordinate>();
                while (coordNumber < sectorData.dataSegments.Count)
                {
                    // Theres only a latitude left, so unparseable - skip
                    if (coordNumber + 1 == sectorData.dataSegments.Count)
                    {
                        this.errorLog.AddEvent(
                            new SyntaxError("Missing visibility center longitude coordinate " + sectorData.dataSegments[0], data.fullPath, i + 1)
                        );
                        coordinateError = true;
                        break;
                    }

                    // Ignore skipped centers
                    if (sectorData.dataSegments[coordNumber] == "" && sectorData.dataSegments[coordNumber + 1] == "")
                    {
                        coordNumber += 2;
                        continue;
                    }

                    Coordinate parsedCoordinate = CoordinateParser.Parse(
                        sectorData.dataSegments[coordNumber],
                        sectorData.dataSegments[coordNumber + 1]
                    );

                    if (parsedCoordinate.Equals(CoordinateParser.invalidCoordinate))
                    {
                        this.errorLog.AddEvent(
                            new SyntaxError("Invalid visibility center " + sectorData.dataSegments[0], data.fullPath, i + 1)
                        );
                        coordinateError = true;
                        break;
                    }

                    parsedCoordinates.Add(parsedCoordinate);
                    coordNumber += 2;
                }

                if (coordinateError)
                {
                    continue;
                }

                // Skip two unused sections
                this.sectorElements.Add(
                    new ControllerPosition(
                        sectorData.dataSegments[0],
                        sectorData.dataSegments[1],
                        sectorData.dataSegments[2],
                        sectorData.dataSegments[3],
                        sectorData.dataSegments[4],
                        sectorData.dataSegments[5],
                        sectorData.dataSegments[6],
                        sectorData.dataSegments[9],
                        sectorData.dataSegments[10],
                        parsedCoordinates,
                        sectorData.comment
                    )
                );
            }
        }
    }
}

﻿using Bogus;
using Compiler.Model;

namespace CompilerTest.Bogus.Factory
{
    static class RouteSegmentFactory
    {
        private static readonly string[] Identifiers = new[] {
            "DIKAS",
            "PENIL",
            "UPGAS",
            "MONTY",
            "KONAN",
            "PEDIG",
            "NORBO",
            "ATSIX"
        };

        public static RouteSegment MakeDoublePoint(string identifier1 = null, string identifier2 = null, string colour = null)
        {
            return new Faker<RouteSegment>()
                .CustomInstantiator(
                    f => new RouteSegment(
                        f.Random.String2(4),
                        new Point(identifier1 ?? f.Random.ArrayElement(Identifiers)),
                        new Point(identifier2 ?? f.Random.ArrayElement(Identifiers)),
                        DefinitionFactory.Make(),
                        DocblockFactory.Make(),
                        CommentFactory.Make(),
                        colour ?? ColourFactory.RandomIdentifier()
                    )
                );
        }
        
        public static RouteSegment MakeDoublePointWithNoColour(string identifier1 = null, string identifier2 = null)
        {
            return new Faker<RouteSegment>()
                .CustomInstantiator(
                    f => new RouteSegment(
                        f.Random.String2(4),
                        new Point(identifier1 ?? f.Random.ArrayElement(Identifiers)),
                        new Point(identifier2 ?? f.Random.ArrayElement(Identifiers)),
                        DefinitionFactory.Make(),
                        DocblockFactory.Make(),
                        CommentFactory.Make()
                    )
                );
        }
        
        public static RouteSegment MakePointCoordinate(string pointIdentifier = null, Coordinate? coordinate = null)
        {
            return new Faker<RouteSegment>()
                .CustomInstantiator(
                    f => new RouteSegment(
                        f.Random.String2(4),
                        new Point(pointIdentifier ?? f.Random.ArrayElement(Identifiers)),
                        coordinate == null ? new Point(CoordinateFactory.Make()) : new Point((Coordinate) coordinate),
                        DefinitionFactory.Make(),
                        DocblockFactory.Make(),
                        CommentFactory.Make()
                    )
                );
        }
        
        public static RouteSegment MakeCoordinatePoint(string pointIdentifier = null, Coordinate? coordinate = null)
        {
            return new Faker<RouteSegment>()
                .CustomInstantiator(
                    f => new RouteSegment(
                        f.Random.String2(4),
                        coordinate == null ? new Point(CoordinateFactory.Make()) : new Point((Coordinate) coordinate),
                        new Point(pointIdentifier ?? f.Random.ArrayElement(Identifiers)),
                        DefinitionFactory.Make(),
                        DocblockFactory.Make(),
                        CommentFactory.Make()
                    )
                );
        }
    }
}

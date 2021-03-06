﻿using Compiler.Model;

namespace Compiler.Validate
{
    public class RoutePointValidator
    {
        /*
         * Validate that a point is a valid point in some way shape or form.
         * To be valid it must be:
         * - A coordinate (always valid)
         * - A defined Fix
         * - A defined NDB
         * - A defined VOR
         * - A defined Airport
         */
        public static bool ValidatePoint(Point point, SectorElementCollection sectorElements)
        {
            return IsValidCoordinate(point) ||
                IsValidVor(point.Identifier, sectorElements) ||
                IsValidNdb(point.Identifier, sectorElements) ||
                IsValidAirport(point.Identifier, sectorElements) ||
                IsValidFix(point.Identifier, sectorElements);
        }

       /*
        * Validate that a point is a valid point in some way shape or form.
        * To be valid it must be:
        * - A defined Fix
        * - A defined NDB
        * - A defined VOR
        * - A defined Airport
        */
        public static bool ValidateEseSidStarPoint(string point, SectorElementCollection sectorElements)
        {
            return IsValidVor(point, sectorElements) ||
                IsValidNdb(point, sectorElements) ||
                IsValidAirport(point, sectorElements) ||
                IsValidFix(point, sectorElements);
        }

        public static bool IsValidCoordinate(Point point)
        {
            return point.Type() == Point.TypeCoordinate;
        }

        public static bool IsValidVor(string identifier, SectorElementCollection sectorElements)
        {
            foreach (Vor vor in sectorElements.Vors)
            {
                if (vor.Identifier == identifier)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidNdb(string identifier, SectorElementCollection sectorElements)
        {
            foreach (Ndb ndb in sectorElements.Ndbs)
            {
                if (ndb.Identifier == identifier)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidAirport(string identifier, SectorElementCollection sectorElements)
        {
            foreach (Airport airport in sectorElements.Airports)
            {
                if (airport.Icao == identifier)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValidFix(string identifier, SectorElementCollection sectorElements)
        {
            foreach (Fix fix in sectorElements.Fixes)
            {
                if (fix.Identifier == identifier)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

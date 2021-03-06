﻿using System;
using Compiler.Event;
using Compiler.Model;
using Compiler.Error;
using System.Collections.Generic;
using Compiler.Argument;

namespace Compiler.Validate
{
    public class AllSidsMustBeUnique : IValidationRule
    {
        public void Validate(SectorElementCollection sectorElements, CompilerArguments args, IEventLogger events)
        {
            List<string> keys = new List<string>();
            foreach (SidStar sidStar in sectorElements.SidStars)
            {
                string sidStarKey = GetKey(sidStar);
                if (keys.Contains(sidStarKey))
                {
                    events.AddEvent(new ValidationRuleFailure("Duplicate SID/STAR " + sidStarKey, sidStar));
                    continue;
                }

                keys.Add(sidStarKey);
            }
        }

        private string GetKey(SidStar sidStar)
        {
            return sidStar.Type + ":" + sidStar.Airport + ":" + sidStar.Runway + ":" +
                sidStar.Identifier + ":" + String.Join(" ", sidStar.Route);
        }
    }
}

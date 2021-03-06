﻿using Bogus;
using Compiler.Model;

namespace CompilerTest.Bogus.Factory
{
    static class FixedColourRunwayCentrelineFactory
    {
        public static FixedColourRunwayCentreline Make()
        {
            return new Faker<FixedColourRunwayCentreline>()
                .CustomInstantiator(
                    _ => new FixedColourRunwayCentreline(
                        RunwayCentrelineSegmentFactory.Make(),
                        DefinitionFactory.Make(),
                        DocblockFactory.Make(),
                        CommentFactory.Make()
                    )
                );
        }
    }
}

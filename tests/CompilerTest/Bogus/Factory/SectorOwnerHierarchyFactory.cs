﻿using System.Collections.Generic;
using Bogus;
using Compiler.Model;

namespace CompilerTest.Bogus.Factory
{
    static class SectorOwnerHierarchyFactory
    {
        public static SectorOwnerHierarchy Make(List<string> controllers = null)
        {
            return GetGenerator(controllers).Generate();
        }

        private static Faker<SectorOwnerHierarchy> GetGenerator(List<string> controllers = null)
        {
            return new Faker<SectorOwnerHierarchy>()
                .CustomInstantiator(
                    _ => new SectorOwnerHierarchy(
                        controllers ?? ControllerPositionFactory.GetIdentifierList(),
                        DefinitionFactory.Make(),
                        DocblockFactory.Make(),
                        CommentFactory.Make()
                    )
                );

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using StructureMap;

namespace InteractR.Resolver.StructureMap
{
    public class ResolverModule : Registry
    {
        public ResolverModule()
        {
            For<IResolver>().Use(context => new StructureMapResolver(context));
            For<IInteractorHub>().Use<Hub>();
        }
    }
}

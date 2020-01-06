# InteractR.Resolver.StructureMap
For documentation see [InteractR](https://github.com/madebykrol/InteractR)

Built for StrucutreMap 4.7.1 for other versions please use this repository for reference and roll your own.

Install from nuget.
```PowerShell
PM > Install-Package InteractR.Resolver.StructureMap -Version 2.1.1
```

# Usage 
Either you can register the resolver yourself.
```Csharp
For<IResolver>().Use(context => new StructureMapResolver(context));
For<IInteractorHub>().Use<Hub>();
```

Or you can use the provided registry
```Csharp
 var container = new Container(c =>
{
    c.AddRegistry<ResolverModule>();
});
```
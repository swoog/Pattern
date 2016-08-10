# Pattern

This projects is libraries to abstract different patterns.

Beta package are publish in [https://www.myget.org/F/swoog/api/v3/index.json]

## Pattern.Core.Interfaces
This package contains a very simple abstraction of an injector.

There are implementation for this abstraction like :
- Pattern.Core (A very simple IOC)
- Pattern.Core.Ninject (Ninject implementation)

You can use all patterns and facilities with this two implementation.

## Pattern.Config
This package is a fluent configurator. With this package you use a fluent syntax like :

```csharp
kernel.Bind<IInterface>().To<Class>()
```

## Pattern.Core.Ninject
Two way is possible to use this package :

On legacy you can :
```csharp
var patternKernel = kernel.BindPattern()
```

Or you can create a StandardKernel like :
```csharp
var patternKernel = new NinjectStandardKernel()
```

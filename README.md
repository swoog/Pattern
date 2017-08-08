# Pattern

This projects is libraries to abstract different patterns.

Beta package are publish in [https://www.myget.org/F/swoog/api/v3/index.json]

![alt text](https://neobd.visualstudio.com/_apis/public/build/definitions/478c14b3-24a1-461b-afbc-95bbcc413b21/20/badge "Build status")

## Pattern.Core.Interfaces
This package contains a very simple abstraction of an injector.

There are implementation for this abstraction like :
- Pattern.Core (A very simple IOC)
- Pattern.Core.Ninject (Ninject implementation)

You can use all patterns and facilities with this two implementation.

#### Pattern.Core
This package is a very simple implementation of IOC.
You can create a kernel like :
```csharp
var kernel = new Kernel();
```

#### Pattern.Core.Ninject
Two way is possible to use this package :

On legacy you can :
```csharp
var patternKernel = kernel.BindPattern()
```

Or you can create a StandardKernel like :
```csharp
var patternKernel = new NinjectStandardKernel()
```

## Pattern.Config
This package is a fluent configurator. With this package you use a fluent syntax like :

```csharp
kernel.Bind<IInterface>
    ().To<Class>
        ()
```


## Pattern.Logging
This package define an interface to implement for abstract error logging in your application.

You can create your own implementation, or you can use :
##### Pattern.Logging.log4net
This package abstract use log4net througth the kernel abstraction in Pattern.Core.Interfaces.

```csharp
    kernel.BindLog4net();
```
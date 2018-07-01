# Pattern

This projects is libraries to abstract different patterns.
Packages are principaly build in .Net Standard for more compatibility.

Beta package are publish in [https://www.myget.org/F/swoog/api/v3/index.json]

Release are on [https://www.nuget.org/]

![alt text](https://neobd.visualstudio.com/_apis/public/build/definitions/478c14b3-24a1-461b-afbc-95bbcc413b21/20/badge "Build status")

## Documentations

You can found documentation on the [wiki](https://github.com/swoog/Pattern/wiki)

## Start example

This is an example of the syntaxt to create a kernel :
```csharp
var kernel = new Kernel();

kernel.Bind<IMotor>().To<ElectricMotor>();
kernel.Bind<Car>().ToSelf();

var car = kernel.Get<Car>();

car.Start();
```
using System;
using Unity;

namespace Pattern.Perf
{
    public class UnityPerfAnalyser : IPerfAnalyser
    {
        private UnityContainer unityContainer;
        public void Create()
        {
            this.unityContainer = new UnityContainer();
        }

        public void Bind()
        {
            this.unityContainer.RegisterType<IMyClass, MyClass>();
            this.unityContainer.RegisterType<IMyClassGeneric<object>, MyClassGeneric<object>>();
        }

        public void Init()
        {
            this.unityContainer.RegisterType<IMyClass, MyClass>();
        }

        public void Get()
        {
            this.unityContainer.Resolve<IMyClass>();
        }

        public string Name => "Unity";
    }
}
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Pattern.Perf
{
    public class AutofacPerfAnalyser : IPerfAnalyser
    {
        private IContainer container;
        private ContainerBuilder containerBuilder;

        public void Create()
        {
            this.containerBuilder = new ContainerBuilder();
        }

        public void Bind()
        {
            this.containerBuilder.RegisterType<MyClass>().As<IMyClass>();
            this.containerBuilder.RegisterType<MyClassGeneric<object>>().As<IMyClassGeneric<object>>();
        }

        public void Init()
        {
            this.container = this.containerBuilder.Build();
        }

        public void Get()
        {
            this.container.Resolve<IMyClass>();
        }
        
        public string Name => "Autofac";
    }
}
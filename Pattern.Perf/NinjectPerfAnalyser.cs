using Microsoft.Extensions.DependencyInjection;
using Ninject;

namespace Pattern.Perf
{
    public class NinjectPerfAnalyser : IPerfAnalyser
    {
        private StandardKernel standardKernel;

        public void Create()
        {
            this.standardKernel = new StandardKernel();
        }

        public void Bind()
        {
            this.standardKernel.Bind<IMyClass>().To<MyClass>();
            this.standardKernel.Bind<IMyClassGeneric<object>>().To<MyClassGeneric<object>>();
        }

        public void Init()
        {
        }

        public void Get()
        {
            this.standardKernel.GetService<IMyClass>();
        }

        public string Name => "Ninject";
    }
}
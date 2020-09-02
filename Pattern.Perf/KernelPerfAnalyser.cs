using Pattern.Config;
using Pattern.Core;
using Pattern.Core.Interfaces;

namespace Pattern.Perf
{
    public class KernelPerfAnalyser : IPerfAnalyser
    {
        private Kernel kernel;

        public void Create()
        {
            this.kernel = new Kernel();
        }

        public void Bind()
        {
            this.kernel.Bind<IMyClass>().To<MyClass>();
            this.kernel.Bind<IMyClassGeneric<object>>().To<MyClassGeneric<object>>();
        }

        public void Init()
        {
        }

        public void Get()
        {
            this.kernel.Get<IMyClass>();
        }

        public string Name => "Pattern";
    }
}
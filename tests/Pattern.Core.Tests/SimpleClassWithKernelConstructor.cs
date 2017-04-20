using Pattern.Core.Interfaces;

namespace Pattern.Core.Tests
{
    public class SimpleClassWithKernelConstructor
    {
        public SimpleClassWithKernelConstructor(IKernel kernel)
        {
            this.Kernel = kernel;
        }

        public IKernel Kernel { get; private set; }
    }
}
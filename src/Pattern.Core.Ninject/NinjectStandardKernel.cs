using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pattern.Core.Ninject
{
    using global::Ninject;

    public class NinjectStandardKernel : Pattern.Core.Interfaces.IKernel
    {
        private readonly StandardKernel standardKernel;

        public NinjectStandardKernel()
        {
            this.standardKernel = new StandardKernel();
        }

        public void Bind(Type @from, Type to)
        {
            this.standardKernel.Bind(from).To(to);
        }

        public object Get(Type @from)
        {
            return this.standardKernel.Get(@from);
        }
    }
}

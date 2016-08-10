using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pattern.Core.Ninject
{
    using global::Ninject;

    public class NinjectStandardKernel : Pattern.Core.Interfaces.IKernel
    {
        private readonly IKernel standardKernel;

        public NinjectStandardKernel()
            : this(new StandardKernel())
        {
            
        }

        public NinjectStandardKernel(IKernel kernel)
        {
            this.standardKernel = kernel;
            this.standardKernel.Bind<Pattern.Core.Interfaces.IKernel>().ToConstant(this);
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

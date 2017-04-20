using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pattern.Core.Ninject
{
    using global::Ninject;
    using global::Ninject.Parameters;

    using Pattern.Core.Interfaces;

    public class NinjectStandardKernel : Pattern.Core.Interfaces.IKernel
    {
        private readonly global::Ninject.IKernel standardKernel;

        public NinjectStandardKernel()
            : this(new StandardKernel())
        {

        }

        public NinjectStandardKernel(global::Ninject.IKernel kernel)
        {
            this.standardKernel = kernel;
            this.standardKernel.Bind<Pattern.Core.Interfaces.IKernel>().ToConstant(this);
        }

        public void Bind(Type @from, IFactory toFactory)
        {
            this.standardKernel.Bind(@from).ToMethod(c => toFactory.Create(new object[0]));
        }

        public bool CanResolve(Type parent, Type @from)
        {
            return (bool)this.standardKernel.CanResolve(@from);
        }

        public object Get(Type parentType, Type @from, params object[] parameters)
        {
            if (parameters.Length > 0)
            {
                throw new NotSupportedException();
            }

            return this.standardKernel.Get(@from);
        }
    }
}
